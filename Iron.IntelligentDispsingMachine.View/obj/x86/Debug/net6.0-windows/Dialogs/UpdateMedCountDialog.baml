   M S B A M L   `   `   `  ����_  [Iron.IntelligentDispsingMachine.View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:Iron.IntelligentDispsingMachine.View.Dialogs,Iron.IntelligentDispsingMachine.View.Dialogs  5   	   K GTouchScreenBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY>clr-namespace:TouchKeyboard.Keyboard;assembly=TouchScreenBoardTouchKeyboard.Keyboard 5   	   G    AIron.IntelligentDispsingMachine.View.Dialogs.UpdateMedCountDialog   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Llocal:clr-namespace:Iron.IntelligentDispsingMachine.View.Dialogs     5   	   Skeyboard>clr-namespace:TouchKeyboard.Keyboard;assembly=TouchScreenBoard     5   	   $��Microsoft YaHei)�5	   
   $��12(�6'   $��
ExtraLight"�65     � WindowStartupLocation$  CenterOwner=�5
   	    � WindowStyle$
 None=�5   	    � Title$ UpdateMedCountDialog��5   	   $	��500��6&   $	��700��63    � 	Resources 5       �� MergedDictionaries �� Source �� 
TargetType �� Property �� Value	 �� 
TargetType
 �� CornerRadius �� Orientation �� RelativeSource �� Mode �� Triggers P� Property P� Value �� 
TargetName�� 5   
    5      �� 5      $f `pack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/Buttonstyles.xaml?�6%   �� 5      $h bpack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/ComboBoxStyles.xaml?�6%   �� 5      $g apack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/DataGridStyle.xaml?�6%   �� 5      �� 5      " ��6   .�5      ��  � ��$	 0,5q�63   �� 5       � �� 5      �� 5   "   "	 ��62   .R�5   &   � .+�5   *   �� ���	#55FFFFFF$
 6��6H   ���#16a1ff$��1q�6o   �� 5   *   �� 5   *   $ 
Horizontal=�65   $	��Top=�6N   $��8q�6f   .+�5    .   �� ���#16a1ff$��3��6J   $��10��6T   $
 2��6`   $��Center=�6q   �� 5!   .   $	��5,0q�6�   #��z��5"   8   $
��Bold"�6^   ���#888��5!   8   �� *Header+ �� $ TemplatedParent=�5"   p   �� 5#   .   ���	#AA16a1ff$��3��6L   $��8��6V   $
 2��6a   $��Center=�6r   �� 5$   .   ���	#7716a1ff$��3��6L   $��8��6V   $
 2��6a   $	��3,0q�6r   $��Center=�6   �� 5%   .   ���	#4416a1ff$��3��6L   $��8��6V   $
 2��6a   $��Center=�6r   �� 5&   .   ���	#1116a1ff$��3��6L   $��8��6V   $
 2��6a   $	��3,0q�6r   $��Center=�6   5'   +   F� 5(   *   7����    ��� �    ��[ ׀�[     ���[ ��[ р��     	���#b4fee7$	��Top=�6e   $
��Left=�6}   $��1q�6�   F� 5)   *   7����    ��� р�[ ��� ׀��  	=  ��[ ��[ р��     	���#b4fee7$��Bottom=�6g   $��Right=�6�   5*   '   5+   #   5-      5.      �� 5/      " ��6   .�50      ��  � ��$ Center=�6E   �� 52       � �� 53      �� 54   "   "	 ��62   .R�55   &   �� $	��bor��6�   
���   �$��1q�6A   $
 3��6U   $��26��6f   
�������._�56   *   �� $��PART_ContentHost��67   $��0q�6O   57   '    58   &   P� 59   *    �@J�IsKeyboardFocused$
 True��6O   . �5:   .   �� $	 bor��65    � �� �#16a1ff6]   5;   +   5=   #   5?      5@      5A      %   '��       '��       5C      '  !System.Windows.Shell.WindowChrome   WindowChrome 5F        5G   
      GlassFrameThickness$ -1q�6   .��5I      �  � RowDefinitions 5J   
   �� 5K      �� 5L      �� 5M      �� 5N      �� 5O      .+�5Q   
   �� $��药品基本信息��6   $��30(�6"   $
��Bold"�60   $��Center=�6B   $��Center=�6_   �� 5R   
   $��1��6   $��药品名称��6!   $��Center=�6-   $��25(�6H   $
��Bold"�6V   $
��Left=�5S   (   $	��5,0q�6C   �� 5T   
    �� 
IsReadOnly$
 True��6   $��1��6$   $��25(�61   $��	128,0,5,0q�6d   $��Center=�6w   ��6?   �� *CurrentMed.MedName+6w   �� 5V   
   $��2��6   $��药品位置��6!   $��Center=�6-   $��25(�6H   $
��Bold"�6V   $
��Left=�5W   (   $	��5,0q�6C   �� 5X   
   $
 True��6   $��2��6$   $��25(�61   $��	128,0,5,0q�6c   $��Center=�6v   ��6?   �� *CurrentMed.MedPos+6v   �� 5Z   
   $��3��6   $��药品位置��6!   $��Center=�6-   $��25(�6H   $
��Bold"�6V   $
��Left=�5[   (   $	��5,0q�6C   �� 5\   
   .  (TouchKeyboard.Keyboard.TouchScreenKeypad   TouchScreenKeypad$
 True��6   $��3��6H   $��25(�6U   $��	128,0,5,0q�6�   $��Center=�6�   ��6d   �� *CurrentMed.MedNowAMT+ �� UpdateSourceTrigger$ Default=�6�   A� 5]   
    A� Rows$ 1��6   $��4��6   .+�5^      �� $��25(�6   $
��Bold"�6#   $��确定��65   $��10q�6�   
��� ���   NormalButtonStyle#��[  6�   ��6B   �� *UpdateCommand+��6d   ��  ��  �� AncestorType
 Window6�   �� 5_      $��25(�6   $
��Bold"�6#   $��取消��65   $��10q�6�   
���  ��  CancelButtonStyle#��[ 6�   ��6B   �� *CancelCommand+��6d   ��  �� 
 Window6�   5`      5a      5b      