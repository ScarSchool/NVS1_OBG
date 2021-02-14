
// Measurements from the file 'messreihe.dat'
const measurements = [3.77,5.89,7.7,10.09,8.98,9.34,10.18,9.55,5.56,5.35,5.05,4.84,0.87,2.27,4.13,2.52,2.45,2.86,7.7,10.82,9.1,13.37,16.47,19.26,17.63,17.49,20.82,18.63,18.98,15.97,17.75,14.45,13.25,111.3,111.7,10.61,11.03,13.98,15.43,18.28,17.42,31.27,44.32,57.12,69.11,37.57,29,31.29,31.07,28.4,27.38,29.15,27.85,25.67,21.73,22.17,24.1,23.54,23.52,23.99,28.87,28.02,-71.23,-67.23,37.63,36.38,39.69,42.5,40.77,40.52,41.82,39.79,37.55,38.26,33.08,35.16,33.63,32.58,32.06,37.07,35.56,87.45,92.62,91.9,95.15,95.21,96.94,101.23,99,103.24,102.96,102.25,100.19,95.95,95.66,95.49,93.59,95.08,96.06,92.57,94.61,96.13,99.04,101.21,102.5,105.75,105.79,109.5,112.76,110.5,111.71,111.41,112.67,111.6,110.34,108.06,104.9];

// Ich musste die letzte 10 auf 20 ändern sonst würde die Störung (Haifischflosse) nicht erkannt werden
let errorPattern = [10, 20, 30, 40, 20];

window.addEventListener('DOMContentLoaded', () => {
    let disturbanceFiltered = patternRemoved(measurements, errorPattern, 40);
    // 90 weil Sie das so verwendet haben
    let spikedFiltered = spikesRemoved(disturbanceFiltered, 90);

    let triangleFiltered = triangleFilter(spikedFiltered);
    let fftFiltered = fftFilter(triangleFiltered, 0.60);

    // Unfiltered
    createChart(document.getElementById('unfilteredChartCanvas'), measurements, '#005FAD', '#00E0CE');
    // Übung 2
    createChart(document.getElementById('uebung2ChartCanvas'), disturbanceFiltered, '#3F9137', '#26E015');
    // Übung 3
    createChart(document.getElementById('uebung3ChartCanvas'), spikedFiltered, '#B38200', '#EBD34B');
    // Übung 4
    uebung4Chart(document.getElementById('uebung4ChartCanvas'), triangleFiltered, fftFiltered);
});


// *********
//  Übung 2
// *********

/**
 * Finds a specified pattern and removes it from an array of raw data
 * @param {Array} rawData 
 * @param {Array} pattern 
 * @param {float} deviationPercent 
 */
function patternRemoved(rawData, pattern, deviationPercent) {
    let filtered = [];
    let index = 0;

    while (index < rawData.length) {
        let found = (rawData.length - index) >= pattern.length;   
        let jndex = index;
        let patternIdx = 0;

        while (found && patternIdx < pattern.length) {
            let expectedLow = pattern[patternIdx] * ((100 - deviationPercent) / 100);
            let expectedHigh = pattern[patternIdx] * ((100 + deviationPercent) / 100);
            let diff = rawData[jndex + 1] - rawData[index];

            found = (diff >= expectedLow && diff <= expectedHigh);

            jndex++;
            patternIdx++;
        }

        if (found) {
            index += pattern.length;
            console.debug(`Found a pattern at index ${index}. Removed the next ${pattern.length} values.`);
        } else {
            filtered.push(rawData[index]);
        }
        index++;
    }

    return filtered;
}

// *********
//  Übung 3
// *********

/**
 * Normalizes data points from an array of data when a change exceeding a certain limit is found
 * @param {Array} measurements 
 * @param {float} limit 
 */
function spikesRemoved(measurements, limit) {
    let measurementsCopy = [...measurements];   // This kind of copy is ok because there are only floats in that array
    let filtered = [];

    let index = 0;
    while (index < measurementsCopy.length) {
        let diff = Math.abs(measurementsCopy[index] - measurementsCopy[index + 1]);
        
        if (diff >= limit) {
            measurementsCopy[index + 1] = measurementsCopy[index];
            console.log(`Found a value exceeding the limit '${limit}' at index '${index}' with a difference of '${diff}'`);
        }
        
        filtered.push(measurementsCopy[index]);
        index++;
    }

    return filtered;
}

// *********
//  Übung 4
// *********

/**
 * Filters data with the well-known triangle filter
 * Formel ist die die Sie in ihrem Excel-Sheet verwendet haben
 */
function triangleFilter(measurements) {
    let filtered = [];

    for(let idx = 0; idx < measurements.length; idx++) {
        if (idx - 2 < 0 || idx + 2 >= measurements.length) {
            filtered.push(measurements[idx]);
        } else {
            let sum = 
                measurements[idx-2]
                + 2 * measurements[idx-1]
                + 3 * measurements[idx] 
                + 2 * measurements[idx+1] 
                + measurements[idx+2];
        
            filtered.push(sum / 9);
        }
    }
    
    return filtered;
}

/**
 * Smooths data through fft
 * Code base from https://rosettacode.org/wiki/Fast_Fourier_transform#JavaScript
 * @param {*} data 
 * @param {*} frequencyDropPercentage 
 */
function fftFilter(data, frequencyDropPercentage) {
    let fftLength = data.length;
    let s = 0;
    while(fftLength > 1) {
      fftLength >>= 1;
      s++;
    }
    
    fftLength <<= ++s;
    const fftPreparedData = new Array(fftLength).fill({ampl: 0, phase: 0});
    data.forEach((v, k) => fftPreparedData[k] = {ampl: v, phase: 0});
    
    return ifft(
      fft(fftPreparedData).map((fft, idx, data) => 
        (Math.min(idx, data.length-idx) < Math.round((1 - frequencyDropPercentage) * data.length/2))
          ? fft
          : {ampl: 0, phase: 0}
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

        fftData[idx + data.length/2] = {
        ampl: evenFft[idx].ampl - t2.ampl,
        phase: evenFft[idx].phase - t2.phase
        };

        return fftData;
    }, new Array(data.length));
}

function ifft(fftData) {
    const invertedPhases = fftData.map(fft => ({ampl: fft.ampl, phase: -fft.phase}));
    return fft(invertedPhases).map(fft => fft.ampl *= (1 / fftData.length));
}

// **********************************
//  Methoden um die Charts zu erzeugen
// **********************************

/**
 * Creates a new chart js chart at the specified canvas with specified data and color options
 * @param {*} canvas 
 * @param {*} data 
 * @param {*} pointColor 
 * @param {*} lineColor 
 */
function createChart(canvas, data, pointColor, lineColor) {
    let chartData = [];
    for (let idx = 0; idx < data.length; idx++) {
        chartData.push({x: idx, y: data[idx]});
    }

    return new Chart(canvas, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Measurement',
                fill: false,
                borderColor: lineColor,
                pointBackgroundColor: pointColor,
                pointBorderColor: pointColor,
                data: chartData,
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
                    scaleLabel: {
                        display: true,
                        labelString: 'Literally just an index'
                    }
                }],
            }
        }
    });
}

/**
 * A kind of special method to create a chart with 2 lines
 * @param {*} canvas 
 * @param {*} triangleFiltered 
 * @param {*} fftFiltered 
 */
function uebung4Chart(canvas, triangleFiltered, fftFiltered) {
    let chartDataFFT = [], chartDataTriangle = [];
    for (let idx = 0; idx < triangleFiltered.length; idx++) {
        chartDataTriangle.push({x: idx, y: triangleFiltered[idx]});
    }
    for (let idx = 0; idx < fftFiltered.length; idx++) {
        chartDataFFT.push({x: idx, y: fftFiltered[idx]});
    }

    return new Chart(canvas, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Triangle filtered',
                fill: false,
                borderColor: '#E34BEB',
                pointBackgroundColor: '#9B00A3',
                pointBorderColor: '#9B00A3',
                data: chartDataTriangle,
            }, {
                label: 'FFT Filtered',
                fill: false,
                borderColor: '#EB5B4B',
                pointBackgroundColor: '#A31000',
                pointBorderColor: '#A31000',
                data: chartDataFFT,
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
                    scaleLabel: {
                        display: true,
                        labelString: 'Literally just an index'
                    }
                }],
            }
        }
    });
}