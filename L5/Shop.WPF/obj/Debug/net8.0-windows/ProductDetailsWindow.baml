   M S B A M L   `   `   `  ����C  ?Shop.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Shop.WPFShop.WPF  5   	   #    Shop.WPF.ProductDetailsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Shop.WPF     5   	     � Title$  ProductDetailsWindow��5   	   $	��450��6&   $	��800��63   .��5
      � $	��Top=�6   $��Stretch=�6#   .+�5   
   ��  �� Orientation$ Vertical=�6   .+�5      �� $��Edycja produktu��6   $��24(�6/   $��Center=�6=   $��0,0,0,20q�6Z   �� 5      $ 
Horizontal=�6   $
��0,10q�62   .+�5      �� $��Nazwa:��6   $��18(�6*   $��Center=�68   $��0,0,10,0q�6S   �� 5      $��18(�68   $��Center=�6F   $	��300��6a   ��6   �� *Product.Name+6a   5      �� 5      $ 
Horizontal=�6   $
��0,10q�62   .+�5      �� $��Cena:��6   $��18(�6)   $��Center=�67   $��0,0,10,0q�6R   �� 5      $��18(�69   $��Center=�6G   $	��300��6b   -  'System.Windows.Input.KeyboardNavigation   TabIndex$ 2��6n   ��6   �� *Product.Price+6n   5      �� 5      $ 
Horizontal=�6   $
��0,10q�62   .+�5      �� $��Ilość:��6   $��18(�6*   $��Center=�68   $��0,0,10,0q�6S   �� 5      $��18(�6A   $��Center=�6O   $	��300��6j   ��6   �� *SelectedStockQuantity+6j   5      �� 5"      $ 
Horizontal=�6   $
��0,10q�62   .+�5#      �� $��
Kategoria:��6   $��18(�6.   $��Center=�6<   $��0,0,10,0q�6W   �� 5$       �� DisplayMemberPath$
 Name��5&      $��18(�5'      $	��300��6)   ��5$      �� *
Categories+ �� SelectedItem 5%      �� *SelectedCategory+5'   )   5(      �� 5+      $��Zapisz zmiany��6   $
��Left=�6T   $	��150��6o   $��0,20,0,0q�6{    J� RenderTransformOrigin$ 1.053,0.669&�6�   ��6-   �� *EditProductCommand+6�   �� 5.      $��Powrót��6   $
��Left=�6H   $	��150��6c   $ 1.373,0.371&�6o   ��6&   �� *GoBackCommand+6o   5/      50      51      