   M S B A M L   `   `   `  ����_  [Iron.IntelligentDispsingMachine.View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:Iron.IntelligentDispsingMachine.View.Windows,Iron.IntelligentDispsingMachine.View.Windows  5   	   a ]Iron.IntelligentDispsingMachine.Assets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�mclr-namespace:Iron.IntelligentDispsingMachine.Assets.Controls;assembly=Iron.IntelligentDispsingMachine.Assets/Iron.IntelligentDispsingMachine.Assets.Controls 5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5	   	   ;    5Iron.IntelligentDispsingMachine.View.Windows.MainView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Llocal:clr-namespace:Iron.IntelligentDispsingMachine.View.Windows     5   	   �controlmclr-namespace:Iron.IntelligentDispsingMachine.Assets.Controls;assembly=Iron.IntelligentDispsingMachine.Assets     5   	   6sys&clr-namespace:System;assembly=mscorlib     5	   	     � 
ResizeMode$  NoResize=�5   	    � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�5   	   $��Microsoft YaHei)�5   	   $��12(�6&   $��
ExtraLight"�64    � WindowStartupLocation$ CenterScreen=�5   	   ���#f1f5fd � Title$! 艾隆科技智联调配机��5   	   $	��800��6   $
��1400��6(    � DataContext 5   	   ��    locator �� Source# [   �� Path$ MainViewModel�'  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
   	   GlassFrameThickness$	 -1q�6   
 � Triggers
 5      :� 5   
    :� RoutedEvent$ UserControl.Loaded��6   .[�5      �� .��5      �� .��5      T�  g� Duration$ 1:0:1D�64    �� 
TargetName$ txt_date��6E    �� TargetProperty$ DataContext�5   4    g� RepeatBehavior$ Forever��6\   ..�5      i� 
 �� Now R� Value# Z 61    R� KeyTime$ 0:0:0��6U   i� 5      # Z 61   $ 1:0:0��6U   5      T� 5      $ 0:0:2D�64   $ txt_time��6E   $ DataContext�5   4   $ Forever��6\   ..�5       i� # Z 61   $ 0:0:0��6U   i� 5!      # Z 61   $ 0:0:1��6U   5"      5#      5$      5%       � 	Resources 5'       �� MergedDictionaries �� Source �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Orientation �� RelativeSource �� Mode  Key ��Name � RowDefinitions  Iconfont  J� Opacity! �� Triggers" P� Property# P� Value$ �� 
TargetName�� 5(   
    5)      �� 5*      $f `pack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/Buttonstyles.xaml?�6%   �� 5+      $h bpack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Styles/ComboBoxStyles.xaml?�6%   �� 5,      �� 5-      " ��6   .�5.      ��  � ��$	 0,5q�63   �� 50       � �� 52      �� 53   "   " ��62   .R�54   &   � .+�55   *   �� ���	#55FFFFFF$ 6��6H   ���	#080FC6F8$��1q�6q   �� 57   *   �� 58   *   $ 
Horizontal=�65   $	��Top=�6N   $��8q�6f   .+�59   .   �� ���#16a1ff$��3��6J   $��10��6T   $ 2��6`   $��Center=�6q   �� 5:   .   $	��5,0q�6�   #��z��5;   8   $
��Bold"�6^   ���#888��5:   8   �� *Header+ �� $ TemplatedParent=�5;   p   �� 5<   .   ���	#AA16a1ff$��3��6L   $��8��6V   $ 2��6a   $��Center=�6r   �� 5=   .   ���	#7716a1ff$��3��6L   $��8��6V   $ 2��6a   $	��3,0q�6r   $��Center=�6   �� 5>   .   ���	#4416a1ff$��3��6L   $��8��6V   $ 2��6a   $��Center=�6r   �� 5?   .   ���	#1116a1ff$��3��6L   $��8��6V   $ 2��6a   $	��3,0q�6r   $��Center=�6   5@   +   F� 5A   *   7����    ��� �    ��[ ׀�[     ���[ ��[ р��     	���#b4fee7$	��Top=�6e   $
��Left=�6}   $��1q�6�   F� 5B   *   7����    ��� р�[ ��� ׀��  	=  ��[ ��[ р��     	���#b4fee7$��Bottom=�6g   $��Right=�6�   5C   '   5D   #   5F      5G      5H      �� 5I      �� 5J      " 	�6   MenuButtonStyle .�5K      ��  � ��$	 5,8q�63   �� 5L       � �� 5M      �� 5N   "   " 	�62   .R�5O   &   �� $
 root��6W   $ 8��6-   
������ $��1q�6e   ._�5P   *   �  5Q   .   �� 5R   2   �� 5S   2   $��24 �6@   .+�5U   .   �� $
��icon��5Z   P   $��Center=�5V   9   $��Center=�5W   9   #��[ 5X   9   $��40(�6`   $
��Bold"�6n   $��0,8,0,0q�5Y   9   ���	#66000000$	  0.7Q�5Z   \   ��5U   9   �� *
MenuIcon+5Z   \   �� 5[   .   $��header��5]   O   $��1��5[   T   $��Center=�5\   8   $��Center=�6U   ���	#99000000$��11(�5]   ]   ��5[   8   �� *
MenuHeader+5]   ]   5^   +   5_   '   ! 5`   &   P� 5a   *   " �@d�	IsChecked$
# TrueY�6G   . �5b   .   �� $
$ root��6]    � �� �	#2216a1ff�� 5c   .   $
$ root��6^    � �� �	#4416a1ff�� 5d   .   $
$ icon��6K    � ��
 � ���6]   5e   +   5g   #   5i      5j      5k      %   '��       '	�       5m      .��5o      �  5s   
   �� 5t      $	��100 �6   �� 5u      % � ColumnDefinitions% 5w   
   �� 5x      $	��140 �6   �� 5y      .+�5}   
   �� $��0,0,0,1q�6   $��2��6-   ��5~      �� 5      & �� 
StartPoint$	& 0,0&�6&   ' �� EndPoint$	' 1,0&�67   .��5�      � ( � Color$( #c2e0fa��6#   ) � Offset$) 0Q�63   � 5�      $( Yellow��6#   $	) 0.6Q�62   5�      5�      �� 5�   
   $��0,0,3,3q�6   $ 0,0,50,0��6,   $	��100��6D   $��2��5�      $��2��6%   $
��Left=�5�      $	��Top=�6,   ��5�      �� 5�      $	& 0,0&�6&   $	' 1,0&�67   .��5�      � $
( blue��6#   $) 0Q�60   � 5�      $( White��6#   $) 1Q�61   5�      ._�5�      �� $ 
Horizontal=�6   .+�5�      �� $��10q�6   $_��Ypack://application:,,,/Iron.IntelligentDispsingMachine.Assets;component/Images/艾隆.png��6&   $
��Fill=�6�   �� 5�      $(��"艾隆科技|智能调配机系统��6   $��1��5�      $��Center=�6,   $��26(�6G   $��5,0,40,0q�5�      $��YouYuan)�6.   * J� SnapsToDevicePixels$
* True��6C   ��5�      �� 5�      $	& 0,0&�6.   $	' 1,1&�6?   .��5�      � $( #16a1ff��6+   $) 0Q�6;   � 5�      $( 	Goldenrod��6+   $
) 0.75Q�6=   � 5�      $( #16a1ff��6+   $) 1Q�6;   5�      + J� Effect+ 5�      3  -System.Windows.Media.Effects.DropShadowEffect  5�      ,   Color$
, Gray��6+   -   ShadowDepth$- 0Q�68   .   
BlurRadius$. 1Q�6I   /   	Direction$/ 0Q�6X   0   Opacity$	0 0.7Q�6f   1 � LayoutTransform1 5�      �� 5�      2 �� ScaleX$	2 1.2Q�6)   5�      5�      5�      � 5�   
   $��1��6   $��Center=�6   % 5�      �� 5�      $��1* �6#   �� 5�      $��5* �6#   .+�5�      �� $��0,0,0,1q�6   $��Center=�61   $��20��6M   $ 3��6Y   $
��14,0q�6j   ._�5�      �� -   5        $��txt_date��5�   4   $��20(�5�      $��Center=�6f   $��Center=�5�       ���	#99444444$��Normal"�5�       # Z 5�       ��5�   *   �� *Now+3 �� Mode$3 OneWay=�4 �� StringFormat$4 yyyy年MM月dd日��5�       5�      �� 5�      -   5        $��txt_time��5�   U   $��Bottom=�5�      $��Center=�65   $��0,0,0,15q�6R   ���#555# Z 5�   +   �� 5�        DigitalDisplay#��[ 6O   $��30(�6|   �6   �� *Now+$3 OneWay=�$4 HH:mm:ss��6|    �� 5�      $� ��6    �� 5�      $��13(�6b   �6   �� *Now+$3 OneWay=�$
4 dddd��5 �� ConverterCulture$5 zh-CN��6b   5�      5�      �� 5�   
   $��1��6z     NormalButtonStyle#��[ 6�   
�������$
��Bold"�6�   $��30(�6�   $��Center=�6�   $��	600,0,0,0q�6  $��	初始化��6  $��80��6&  $	��200��62  {�6   �� *InitBtnEnable+y�65   �� *
InitBtmVis+��6W   �� *InitSysCommand+62  �� 5�   
   $ 
Horizontal=�6   $	��Top=�6.   $��Right=�5�      $��1��61   $��0,0,5,0q�5�      
������ 6   IsHitTestVisibleInChrome$
6 True��5�   .   .+�5�      �� -   5        $	����5�        WindowControlButtonStyle#��[ 6(   #��[ 6Z   $��40��5�   ,   $��50��68   $��20(�6C   ���#555$��0,10,0,0q�6c   �� 5�      -   5        $	����5�      $��20(�6(   #��[ 66   #��[ 6h   $��40��5�   ,   $��50��68   ���#555$��0,10,0,0q�6U   �� 5�      -   5        $	����5�      $��20(�6(   #��[ 66   #��[ 6h   $��40��5�      $��50��6!   ���#555$��0,10,0,0q�6>   6Q   5�      � 5�   
   $��1��6   $��1��6   .+�5�      � ��5�      !� 5�      7 j� Viewport$7 	0,0,100,6�6"   8 j� ViewportUnits$8 Absolute=�67   9 j� TileMode$
9 Tile=�6P   : !� Visual: 5�      F� 5�      O���Ѐ� ���    ���р�    р��    �@����р�� ���@����� �����	���	#11ff6e00$��1��6l   5�      ; J� OpacityMask; 5�      
� 5�      .��5�      � $( Black��6'   $	) 0.3Q�65   � 5�      $( Transparent��6'   $) 1Q�6;   5�      5�      �� 5�      $	��0,5q�6>   ��6   �� *ViewContent+6>   5�      � 5�   
   $��1��6   .+�5�      �� $��0,0,1,0q�6   ��5�      �� 5�      $	& 0,0&�6*   $	' 0,1&�6;   .��5�      � $( 	#1116a1ff��6'   $	) 0.1Q�69   � 5�      $( 	#1116a1ff��6'   $	) 0.2Q�69   � 5�      $( 	#1116a1ff��6'   $	) 0.5Q�69   � 5�      $( Transparent��6'   $) 1Q�6;   5�      ��5�      �� 5�      $	& 0,0&�6*   $	' 0,1&�6;   .��5�      � $( Transparent��6'   $) 0Q�6;   � 5�      $
( Gold��6'   $	) 0.3Q�64   � 5�      $( Transparent��6'   $) 1Q�6;   5�      ._�5�      �� $��Center=�6   $��	0,0,0,100q�6Y   ��6;   �� *Menus+��5�      �� 5      .R�5     	� < 	� 	GroupName$
< menu��6K     MenuButtonStyle#��[ 5  *   = d� 	IsChecked= 5  *   �� *
IsSelected+��5  *   �� *DataContext.SwitchPageCommand+ �� > �� AncestorType
> Window��5  *   �� 5     5     5     5	     A  ;Iron.IntelligentDispsingMachine.Assets.Controls.Progressing  5
  
   $��Center=�6   ?   Message$? 电机正在回原点��6;   $��Center=�6p   $��2��6�   $��2��6�   y�6M   �� *ProgressVis+6�   5     5     