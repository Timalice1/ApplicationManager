   M S B A M L   `   `   `  ����N  JApplication Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Application_ManagerApplication_Manager  5   	   $    Application_Manager.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:Application_Manager     5   	   -   5          � Title$  App Manager��5   	   $	��600��6   $	��573��6*    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowState$ Normal=�5
        � WindowStyle$ SingleBorderWindow=�65    � 
ResizeMode$ NoResize=�6V   .��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   ���	#FFE8E6E6.+�5      �� -   5        $��btnUninstall��5      $��	Uninstall��6)   $��5q�6=   {�.  5      5      �� 5   
   -   5        $��appList��5       $��1��6    �� View 5      �� 5      .P�5      ��  �� Width$	 150��6%   	 �� DisplayMemberBinding	 61   �� 
 �� Path$

 Name�.��6\   Name6b   �� 5      $	 100��6%   	 61   �� $
 Version�.��6_   	Version6h   �� 5      $	 100��6%   	 61   �� $

 Size�.��6\   Size6b   �� 5      $	 100��6%   	 61   �� $
 	Publisher�.��6a   	Publisher6l   �� 5      $	 100��6%   	 61   �� $
 InstallationDate�.��6h   Install Time6v   5       5"      5$      5%      