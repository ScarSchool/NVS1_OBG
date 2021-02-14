const measurements = [3.77, 5.89, 7.7, 10.09, 8.98, 9.34, 10.18, 9.55, 5.56, 5.35, 5.05, 4.84, 0.87, 2.27, 4.13, 2.52, 2.45, 2.86, 7.7, 10.82, 9.1, 13.37, 16.47, 19.26, 17.63, 17.49, 20.82, 18.63, 18.98, 15.97, 17.75, 14.45, 13.25, 111.3, 111.7, 10.61, 11.03, 13.98, 15.43, 18.28, 17.42, 31.27, 44.32, 57.12, 69.11, 37.57, 29, 31.29, 31.07, 28.4, 27.38, 29.15, 27.85, 25.67, 21.73, 22.17, 24.1, 23.54, 23.52, 23.99, 28.87, 28.02, -71.23, -67.23, 37.63, 36.38, 39.69, 42.5, 40.77, 40.52, 41.82, 39.79, 37.55, 38.26, 33.08, 35.16, 33.63, 32.58, 32.06, 37.07, 35.56, 87.45, 92.62, 91.9, 95.15, 95.21, 96.94, 101.23, 99, 103.24, 102.96, 102.25, 100.19, 95.95, 95.66, 95.49, 93.59, 95.08, 96.06, 92.57, 94.61, 96.13, 99.04, 101.21, 102.5, 105.75, 105.79, 109.5, 112.76, 110.5, 111.71, 111.41, 112.67, 111.6, 110.34, 108.06, 104.9];
const errorPattern = [10, 20, 30, 40, 20]

window.addEventListener('DOMContentLoaded', () => {
    drawChartWithoutIndex(document.getElementById('unfilteredChartCanvas'), unfilteredChart(measurements), "grey")
    drawChartWithoutIndex(document.getElementById('filteredChartCanvas-Pattern'), filterChartByPattern(measurements, errorPattern, 15), "blue")

    drawChartWithoutIndex(document.getElementById('filteredChartCanvas-Spikes'), filterSpikesFromChart(measurements, 90), "green")

    drawChartWithoutIndex(document.getElementById('filteredChartCanvas-FFT'), filterChartWithFFT(measurements, 90, errorPattern, 15, 0.60), "red")

});

function unfilteredChart(data) {
    let returndata = [];
    for (let index = 0; index < data.length; index++)
        returndata.push(data[index]);


    return returndata
}

function filterSpikesFromChart(data, spikeHeight) {
    let dataCopy = [...data];
    let filteredData = [];


    for (let index = 0; index < dataCopy.length; index++) {
        let diff = Math.abs(dataCopy[index] - dataCopy[index + 1]);

        if (diff >= spikeHeight) {
            dataCopy[index + 1] = dataCopy[index];
        }

        filteredData.push(dataCopy[index]);
    }

    return filteredData
}

function filterChartByPattern(data, pattern, deviation) {
    let filteredData = [];

    for (let chartPos = 0; chartPos < data.length; chartPos++) {
        let index = chartPos, patternIndex = 0;
        let followsPattern = true;

        while (patternIndex < pattern.length && followsPattern && chartPos < data.length) {

            let valueDiff = data[index + 1] - data[chartPos];

            followsPattern = valueDiff < (deviation + pattern[patternIndex]) && valueDiff > (pattern[patternIndex] - deviation)

            patternIndex++; index++;
        }

        if (followsPattern) {
            chartPos += pattern.length;
        } else {
            filteredData.push(data[chartPos]);
        }
    }

    return filteredData
}


/**
 * Filters data with the well-known triangle filter
 * Formel ist die die Sie in ihrem Excel-Sheet verwendet haben
 */
function triangleFilter(data) {
    let filtered = [];

    for (let idx = 0; idx < data.length; idx++) {
        if (idx - 2 < 0 || idx + 2 >= data.length) {
            filtered.push(data[idx]);
        } else {
            let sum =
                data[idx - 2]
                + 2 * data[idx - 1]
                + 3 * data[idx]
                + 2 * data[idx + 1]
                + data[idx + 2];

            filtered.push(sum / 9);
        }
    }

    console.log(data)

    return filtered;
}

/**
 * Smooths data through fft
 * Code base from https://rosettacode.org/wiki/Fast_Fourier_transform#JavaScript
 * @param {*} measurements 
 * @param {*} frequencyDropPercentage 
 */
function fftFilter(data, frequencyDropPercentage) {
    let fftLength = data.length;
    let s = 0;
    while (fftLength > 1) {
        fftLength >>= 1;
        s++;
    }

    fftLength <<= ++s;
    const fftPreparedData = new Array(fftLength).fill({ ampl: 0, phase: 0 });
    data.forEach((v, k) => fftPreparedData[k] = { ampl: v, phase: 0 });

    return ifft(
        fft(fftPreparedData).map((fft, idx, data) =>
            (Math.min(idx, data.length - idx) < Math.round((1 - frequencyDropPercentage) * data.length / 2))
                ? fft
                : { ampl: 0, phase: 0 }
        )
    ).slice(0, data.length);
}

function fft(data) {
    if (data.length <= 1) return data;

    const evenFft = fft(data.filter((_, idx) => idx % 2 === 0));
    const oddFft = fft(data.filter((_, idx) => idx % 2 !== 0));
    return evenFft.reduce((fftData, _, idx) => {
        const t1 = {
            ampl: Math.cos((-2 * Math.PI) * (idx / data.length)),
            phase: Math.sin((-2 * Math.PI) * (idx / data.length))
        };

        const t2 = {
            ampl: t1.ampl * oddFft[idx].ampl - t1.phase * oddFft[idx].phase,
            phase: t1.ampl * oddFft[idx].phase + t1.phase * oddFft[idx].ampl
        };

        fftData[idx] = {
            ampl: evenFft[idx].ampl + t2.ampl,
            phase: evenFft[idx].phase + t2.phase
        };

        fftData[idx + data.length / 2] = {
            ampl: evenFft[idx].ampl - t2.ampl,
            phase: evenFft[idx].phase - t2.phase
        };

        return fftData;
    }, new Array(data.length));
}

function ifft(fftData) {
    const invertedPhases = fftData.map(fft => ({ ampl: fft.ampl, phase: -fft.phase }));
    return fft(invertedPhases).map(fft => fft.ampl *= (1 / fftData.length));
}


function filterChartWithFFT(data, spikeHeight, pattern, deviation, frequency) {
    let triangleFiltered = triangleFilter(filterSpikesFromChart(filterChartByPattern(data, pattern, deviation)), spikeHeight);
    let fftFiltered = fftFilter(triangleFiltered, frequency)
    return fftFiltered;
}

function drawChartWithoutIndex(canvas, data, color) {
    let displayData = []
    for (let index = 0; index < data.length; index++) {
        displayData.push({ x: index, y: data[index] });

    }

    new Chart(canvas, {
        type: 'line',
        data: {
            datasets: [{
                label: "data",
                fill: false,
                borderColor: color,
                pointBackgroundColor: "light" + color,
                pointBorderColor: color,
                data: displayData
            }],
        },
        options: {
            elements: {
                line: {
                    tension: 0
                }
            },
            scales: {
                xAxes: [{
                    type: 'linear',
                    position: 'bottom',
                }],
            }
        }
    });
}

function drawChart(canvas, data, color) {
    new Chart(canvas, {
        type: 'line',
        data: {
            datasets: [{
                label: "data",
                fill: false,
                borderColor: color,
                pointBackgroundColor: "light" + color,
                pointBorderColor: color,
                data: data
            }],
        },
        options: {
            elements: {
                line: {
                    tension: 0
                }
            },
            scales: {
                xAxes: [{
                    type: 'linear',
                    position: 'bottom',
                }],
            }
        }
    });
}