   M S B A M L   `   `   `  ����F  BCalcBinding, Version=2.5.2.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:CalcBinding;assembly=CalcBindingCalcBinding  5   	   _ [Iron.IntelligentDispsingMachine.View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:Iron.IntelligentDispsingMachine.View.Dialogs,Iron.IntelligentDispsingMachine.View.Dialogs 5   	   =   7Iron.IntelligentDispsingMachine.View.Dialogs.LedSetView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   <c.clr-namespace:CalcBinding;assembly=CalcBinding     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Llocal:clr-namespace:Iron.IntelligentDispsingMachine.View.Dialogs     5   	   $��Microsoft YaHei)�5	   
   $��12(�6'   $��
ExtraLight"�65     � WindowStartupLocation$  CenterOwner=�5
   	    � Title$ 
LedSetView��5   	   $	��450��6   $	��800��6)    � DataContext 5      ��    locator �� Source# [   �� Path$ LedSetViewModel���5      �� 5   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 0,1&�6/   .��5      �  � Color$ #DCEDfa��6    � Offset$ 0Q�6+   � 5      $ #F7F9FA��6   $	 0.5Q�6+   � 5      $ 	LightBlue��6   $ 1Q�6-   5      	 � 	Resources	 5      
 �� MergedDictionaries �� Source�� 5   
   
 5      �� 5      $f `pack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/Buttonstyles.xaml?�6%   �� 5      $h bpack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/ComboBoxStyles.xaml?�6%   %    5      '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 1q�6   .��5!      � .+�5"   
   �  � RowDefinitions 5#      �� 5$      $��1* �6    �� 5%      $��4* �6    �� 5&      $��1* �6    .+�5(      A�  A� Rows$ 1��6   $
��Left=�6#   .+�5)      �� $��电路板ID:��6   $��30(�6*   $��Right=�68   $
��Bold"�6T   $��20,0,0,0q�6g   $��Center=�6y   �� 5*        PropComboBoxStyle#��[ 6@   $��10,0,0,0q�6l   $��30(�6~   $
��Bold"�6�   $��Center=�6�    �� SelectedIndex 6   �� *
PCBIdIndex+.@�5+      �� $��1��6#   �� 5,      $��2��6#   �� 5-      $��3��6#   �� 5.      $��4��6#   �� 5/      $��5��6#   �� 50      $��6��6#   �� 51      $��7��6#   �� 52      $��8��6#   �� 53      $��9��6#   �� 54      $��10��6#   �� 55      $��11��6#   �� 56      $��12��6#   57      58      � 59      $��1��6    � ColumnDefinitions 5:      �� 5;      $��1* �6'   �� 5<      $��8* �6'   .+�5>      A�  A� Columns$ 1��6   $��0,0,0,15q�6*   .+�5?      �� $��01��6@   $��30(�6M   $
��Bold"�6[   $��Center=�6m   $��Center=�6�   $��Center=�6�   $��Center=�6�    d� 	IsChecked 6   �� *
FirstCheck+6�   �� 5@      $��02��6A   $��Center=�6N   $��30(�6k   $
��Bold"�6y   $��Center=�6�   $��Center=�6�    6   �� *SecondCheck+6�   �� 5A      $��03��6@   $��Center=�6M   $��30(�6j   $
��Bold"�6x   $��Center=�6�   $��Center=�6�    6   �� *
ThirdCheck+6�   5B      �� 5C      $��1��6   ��6/   �� *CabineModels+��5D      �� 5E      .R�5F      A� $ 1��6*   5G      ��5I      �� 5J      .R�5K      �� $��10q�6%   $��1q�61   
���   �._�5L   "   �� $��it��6Q   ��6/   �� *	ledModels+��5M   &   �� 5N   *   .R�5O   .   A� $ 3��6;   5P   +   ��5R   &   �� 5S   *   .R�5T   .   � .+�5U   2   ��  �� Orientation$ Vertical=�6=   .+�5V   6   �� $��Center=�6S   ��6@   �� *X+6S   �� 5W   6   $	��bor��6=    �� CornerRadius$ 20��6H   $��40��6Z   $��40��6e   
�������5Y   7   �� 5Z   6   $��Center=�6S   ��6@   �� *Y+6S   5[   3   5]   /    �� Triggers 5^   .   �� 5_   2    �� Value$
 true��6Z    �� Binding 6>   �� *	IsLight+.a�5`   6   ��  �� 
TargetName$	 bor��6=    �� Property � �� �� Value
 �  ��6d   5a   3   5c   +   5e   #   5f      5g      5j      5k      A� 5l      $��2��6   $ 1��6)   .+�5m      �� $��20(�6=   $
��Bold"�6K   
���   �$��选中亮灯��6p   $��10q�6   
���  ��  NormalButtonStyle#��[ 6�   ��6   �� *OpenLedByChoose+6�   �� 5n      $��20(�68   $
��Bold"�6F   
���   �$��全部亮灯��6k   $��10q�6z   
��� � �#��[ 6�   ��6   �� *
OpenAllLed+6�   �� 5o      $��20(�6<   $
��Bold"�6J   
���   �$��全部灭灯��6o   $��10q�6~   
��� ���#��[ 6�   ��6   �� *LightOffAllLed+6�   �� 5p      $��20(�63   $
��Bold"�6A   
���   �$��烧制ID��6f   $��10q�6u   
��� ���#��[ 6�   ��6   �� *SetID+6�   �� 5q      -   5        $��20(�5q      $
��Bold"�6'   
���   �$��退出��6L   $��10q�6Y   
�������#��[ 6w   6�   5r      5s      5u      5v      