   M S B A M L   `   `   `  ����D  @301295955, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:_301295955
_301295955  5   	        _301295955.BooksListWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:_301295955     5   	   -   5          � Title$  BooksListWindow��5   	   $	��450��6!   $	��900��6.    � 	Resources 5
      &    _301295955.ListToStringConverter   ListToStringConverter��%   &	          5   
   6&   5      .��5      � .+�5   
   �� .+�5      �� $
��auto��6   
����i��$��60��6:   $	��Top=�6F   .@�5      �  � ColumnDefinitions 5      �� 5      $
��Auto �6+   �� 5      $��* �6+   .+�5      �� -   5         ��Name$ 	Hello_lbl��5      $��Hello��6/   $
��Left=�6?   $��10,0,0,0q�6Z   $��Center=�6l   $��16(�6�   $��0��6�   �� 5      -   5        $ userName_lbl��5      $	��173��62   $��16(�6>   ���	#FF611AB9$
��Bold"�6c   $��Center=�6u   $��1��6�   5      5      �� 5      $��1��6   $��0,20,0,0q�6(   ��6:   �� *
BookTitles+��5      �� 5      .R�5      �  � Orientation$ 
Horizontal=�6$   5       ��5"      �� 5#      .R�5$      �� -   5        #��[" �5$   !   
������ 
������ .��5%      ��  �� Padding$ 10q�6%   
����i��$��1q�6H   $��10,10,10,10q�6\   ._�5&   "   �  � RowDefinitions 5'   &   �� 5(   *   $
��auto �68   �� 5)   *   $
��auto �68   .+�5+   &   �� $
��Bold"�6K   $��16(�6]   ��60   �� *	BookTitle+6]   �� 5,   &   $��1��60   ��6=   �� *Author+ �� 	Converter# [  5-   #   5.      5/      50      52      53      54      55      