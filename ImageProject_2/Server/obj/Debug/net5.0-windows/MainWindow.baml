   M S B A M L   `   `   `  ����F  BImageServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ImageServerImageServer  5   	       ImageServer.MainWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ImageServer     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��900��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��2* �6   �� 5      $��2* �6    � RowDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ 
pnlOptions��5      .+�5      �� -   5         ��Name$ btnChooseImage��5      $��0 0 0 20q�6-   $��50��6?   $��Bild auswählen��6K   6d   �� 5      -   5        $ btnUploadImage��5      $��0 0 0 20q�6-   $��50��6?   $��Bild senden��6K   6a   �� 5      -   5         ��Name$ lblRed��5      $��
Rotanteil:��6$   $��0 0 0 10q�69   �� 5      -   5         ��Name$ 	sliderRed��5       � Minimum$ 0Q�6(    � Maximum$	 255Q�64   	 �� AutoToolTipPlacement$	 TopLeft=�6B   
 �� AutoToolTipPrecision$
 0��6a    �� TickFrequency$ 5Q�6z    �� IsSnapToTickEnabled$
 True��6�    �� TickPlacement$ BottomRight=�6�   6�   �� 5      -   5        $ lblGreen��5      $��Grünanteil:��6&   $��0 0 0 10q�6<   �� 5      -   5        $ sliderGreen��5      $ 0Q�6*   $	 255Q�66   $	 TopLeft=�6E   $
 0��6d   $ 5Q�6}   $
 True��6�   $ BottomRight=�6�   6�   �� 5      -   5        $ lblBlue��5      $��Blauanteil:��6%   $��0 0 0 10q�6;   �� 5      -	   5        $ 
sliderBlue��5      $ 0Q�6)   $	 255Q�65   $	 TopLeft=�6D   $
 0��6c   $ 5Q�6|   $
 True��6�   $ BottomRight=�6�   6�   5      �� 5   
   -
   5         ��Name$ 
imgPreview��5      $��1��6$   $
��None=�64   �� 5   
   -   5        $ 
afterImage��5      $��2��6$   $
��None=�64   5      5      