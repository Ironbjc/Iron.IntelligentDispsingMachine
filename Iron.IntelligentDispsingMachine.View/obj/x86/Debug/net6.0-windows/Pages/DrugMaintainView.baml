   M S B A M L   `   `   `  ����_  [Iron.IntelligentDispsingMachine.View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullg8clr-namespace:Iron.IntelligentDispsingMachine.View.Pages*Iron.IntelligentDispsingMachine.View.Pages  5      K GTouchScreenBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY>clr-namespace:TouchKeyboard.Keyboard;assembly=TouchScreenBoardTouchKeyboard.Keyboard 5      A    ;Iron.IntelligentDispsingMachine.View.Pages.DrugMaintainView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jlocal8clr-namespace:Iron.IntelligentDispsingMachine.View.Pages     5      Skeyboard>clr-namespace:TouchKeyboard.Keyboard;assembly=TouchScreenBoard     5        � DataContext  5
      ��    locator �� Source# [   �� Path$ DrugMaintainViewModel� � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius	 �� Triggers
 P� Property P� Value �� 
TargetName  ControlButtonStyle �� RelativeSource �� Mode �� MergedDictionaries �� Source �� Orientation�� 5   
    56      �� 57      $f `pack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/Buttonstyles.xaml?�6%   �� 58      $h bpack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/ComboBoxStyles.xaml?�6%   �� 59      $g apack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/DataGridStyle.xaml?�6%   �� 5:      �� 5;      " ��6   .�5<      ��  � ��$	 0,5q�63   �� 5=       � �� 5?      �� 5@   "   " ��62   .R�5A   &   � .+�5B   *   �� ���	#55FFFFFF$ 6��6H   ���#16a1ff$��1q�6o   �� 5D   *   �� 5E   *   $ 
Horizontal=�65   $	��Top=�6N   $��8q�6f   .+�5F   .   �� ���#16a1ff$��3��6J   $��10��6T   $ 2��6`   $��Center=�6q   �� 5G   .   $	��5,0q�6�   #��z��5H   8   $
��Bold"�6^   ���#888��5G   8   �� *Header+ �� $ TemplatedParent=�5H   p   �� 5I   .   ���	#AA16a1ff$��3��6L   $��8��6V   $ 2��6a   $��Center=�6r   �� 5J   .   ���	#7716a1ff$��3��6L   $��8��6V   $ 2��6a   $	��3,0q�6r   $��Center=�6   �� 5K   .   ���	#4416a1ff$��3��6L   $��8��6V   $ 2��6a   $��Center=�6r   �� 5L   .   ���	#1116a1ff$��3��6L   $��8��6V   $ 2��6a   $	��3,0q�6r   $��Center=�6   5M   +   F� 5N   *   7����    ��� �    ��[ ׀�[     ���[ ��[ р��     	���#b4fee7$	��Top=�6e   $
��Left=�6}   $��1q�6�   F� 5O   *   7����    ��� р�[ ��� ׀��  	=  ��[ ��[ р��     	���#b4fee7$��Bottom=�6g   $��Right=�6�   5P   '   5Q   #   5S      5T      5U      %/  '��       &	 �    �� 5      " ��6   .�5      ��  � ��$ Center=�6=   �� 5       � �� 5      �� 5      " ��6*   .R�5      �� $	��bor��6�   
���   �$��1q�69   $ 3��6M   $��26��6^   
�������._�5   "   �� $��PART_ContentHost��6/   $��0q�6G   5      	 5      P� 5   "   
 �@J�IsKeyboardFocused$
 True��6G   . �5   &   �� $	 bor��6-    � �� �#16a1ff6U   5   #   5      5      5      �� 5      " ��6   .�5       ��  � �� �#8886/   �� 5!       � �� 5"      �� 5#      " ��6*   .R�5$      �� $
��back��6>   
������ ._�5%   "   �� $	��bor��6{    6)   �� *Tag+ �� $ TemplatedParent=�._�5&   &   �� $��Center=�67   #��z��6R   5'   #   5(      	 5)      P� 5*   "   
 �@J�IsMouseOver$
 True��6A   . �5+   &   �� $	 bor��6-    � �� �	#090000006T   5,   #   P� 5-   "   
 � ��$
 True��6?   . �5.   &   �� $	 bor��6-    � �� �	#110000006T   5/   #   51      53      54      '��       5W      .��5Y      �  � RowDefinitions 5Z   
   �� 5[      $��1* �6   �� 5\      $��5* �6   �� 5]      $��1* �6   .+�5_   
   �� $��1q�6   
���   �$��10q�69   ._�5`      �  � ColumnDefinitions 5a      �� 5b      $��1* �6'   �� 5c      $
��1.5* �6'   �� 5d      $
��1.2* �6'   .+�5f      �� $��药品名称��6   $��Center=�6(   $��25(�6C   $
��Bold"�6Q   $
��Left=�5g   (   $	��5,0q�6C   �� 5h      0  *TouchKeyboard.Keyboard.TouchScreenKeyboard   TouchScreenKeyboard$
 True��6   $��25(�6R   $��	128,0,5,0q�6�   $��Center=�6�   ��6`   �� *PYCode+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5i      $��1��6   $��药品条形码��6,   $��Center=�69   $��25(�6T   $
��Bold"�6b   $
��Left=�5j   (   $	��5,0q�6C   �� 5k      $��1��6    �� 
IsReadOnly$
 True��6+   $��25(�6=   $��	148,0,5,0q�6�   $��Center=�6�   ��6K   �� *	BarCode+$ PropertyChanged=�6�   �� 5l      $��2��6   $��药品监管码��6,   $��Center=�69   $��25(�6T   $
��Bold"�6b   $
��Left=�5m   (   $	��5,0q�6C   �� 5n      $��2��6   $
 True��6+   $��25(�6=   $��	148,0,5,0q�6�   $��Center=�6�   ��6K   �� *SupervisoryCode+$ PropertyChanged=�6�   5o      5p      �� 5q   
   $��1��6   $��1q�6   
���   �$��10q�6F   ._�5r      &   System.Windows.Controls.DataGrid     SelectionMode$ Single=�6   $��20(�6.   $
��Bold"�6<    �� SelectedItem 6N   �� *CurrentSelectDrug+��6y   �� *DrugDetailList+   Columns 5t      0  *System.Windows.Controls.DataGridTextColumn  5u      ,  &System.Windows.Controls.DataGridColumn   Header$ 药品名称��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$
 1.5* 67   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   �� *	MedName+  5v      $ 药品厂家��6)   $ * 67    6A   �� *
MedFactory+  5w      $ 药品规格��6)   $
 0.5* 67    6D   �� *	MedUnit+  5x      $ 药品条形码��6)   $ * 68    6B   �� *
MedBarCode+  5y      $ 药品监管码��6)   $ * 68    6B   �� *MedMonitorCode+5|      5}      �� 5~   
   $��2��6   $��1q�6   
���   �$��10q�6F   ._�5      �  5�      �� 5�      �� 5�      �� 5�      .+�5�      �� $��1��68   $��20(�6H   $
��Bold"�6V     NormalButtonStyle#��[ 6h   
���  ��$��删除条码��6�   $��10q�6�   ��6   �� *
DeleteCode+6�   �� 5�      $��2��66   $��20(�6F   $
��Bold"�6T   #��[ 6f   
��� � �$��保存条码��6�   $��10q�6�   ��6   �� *
SaveCode+6�   5�      5�      5�      5�      