using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Abstractions.Controls;

namespace yd_pmsApp1.ViewModels.Pages
{
    public partial class PrintTestViewModel : ObservableObject, INavigationAware
    {
        // 私有字段，用于存储字体大小
        private double _fontSize = 14;
        [ObservableProperty]
        private string _labelTitle = "测试标签";

        [ObservableProperty]
        private string _labelContent = "这是一个测试标签内容，用于打印测试。";

        [ObservableProperty]
        private int _selectedSizeIndex = 1; // 默认选中中标签

        // 修改 FontSize 属性以确保值总是整数
        public double FontSize
        {
            get => _fontSize;
            set
            {
                // 将值四舍五入到最接近的整数
                double roundedValue = Math.Round(value);

                // 确保值在有效范围内（8-24）
                roundedValue = Math.Max(8, Math.Min(24, roundedValue));

                // 设置值并通知UI
                SetProperty(ref _fontSize, roundedValue);

                // 更新预览
                RefreshPreview();
            }
        }

        [ObservableProperty]
        private bool _showDateTime = true;

        [ObservableProperty]
        private bool _showBorder = true;

        [ObservableProperty]
        private bool _showQRCode = true;

        [ObservableProperty]
        private UIElement? _labelPreview = null; // 修改为可空类型

        public PrintTestViewModel()
        {
            // 初始化时生成预览
            RefreshPreview();
        }

        public Task OnNavigatedFromAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnNavigatedToAsync()
        {
            // 页面加载时生成预览
            RefreshPreview();
            return Task.CompletedTask;
        }

        // 刷新预览
        [RelayCommand]
        private void RefreshPreview()
        {
            // 获取标签尺寸
            Size labelSize = GetLabelSize();

            // 创建标签容器
            Border labelContainer = new Border
            {
                Width = labelSize.Width,
                Height = labelSize.Height,
                Background = Brushes.White,
                BorderBrush = ShowBorder ? Brushes.Black : Brushes.Transparent,
                BorderThickness = ShowBorder ? new Thickness(1) : new Thickness(0),
                Padding = new Thickness(10)
            };

            // 标签内容
            Grid contentGrid = new Grid();
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // 标题
            TextBlock titleBlock = new TextBlock
            {
                Text = LabelTitle,
                FontSize = FontSize + 2,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            Grid.SetRow(titleBlock, 0);
            contentGrid.Children.Add(titleBlock);

            // 主要内容
            Grid mainContent = new Grid();
            mainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = ShowQRCode ? new GridLength(2, GridUnitType.Star) : new GridLength(1, GridUnitType.Star) });

            if (ShowQRCode)
            {
                mainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // 标签内容文本
            TextBlock contentBlock = new TextBlock
            {
                Text = LabelContent,
                FontSize = FontSize,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(contentBlock, 0);
            mainContent.Children.Add(contentBlock);

            // 添加二维码（如果启用）
            if (ShowQRCode)
            {
                Border qrCodeBorder = CreateQRCodePlaceholder();
                Grid.SetColumn(qrCodeBorder, 1);
                mainContent.Children.Add(qrCodeBorder);
            }

            Grid.SetRow(mainContent, 1);
            contentGrid.Children.Add(mainContent);

            // 添加日期时间（如果启用）
            if (ShowDateTime)
            {
                TextBlock dateTimeBlock = new TextBlock
                {
                    Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    FontSize = FontSize - 2,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                Grid.SetRow(dateTimeBlock, 2);
                contentGrid.Children.Add(dateTimeBlock);
            }

            // 设置标签内容
            labelContainer.Child = contentGrid;
            LabelPreview = labelContainer;
        }

        // 创建二维码占位符
        private Border CreateQRCodePlaceholder()
        {
            // 创建一个简单的二维码模拟图
            Grid qrCode = new Grid
            {
                Width = 80,
                Height = 80
            };

            // 黑色背景
            Rectangle background = new Rectangle
            {
                Fill = Brushes.Black,
                Width = 80,
                Height = 80
            };
            qrCode.Children.Add(background);

            // 添加一些白色块来模拟QR码图案
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (random.Next(2) == 0) // 50%的概率添加白色块
                    {
                        Rectangle whiteBlock = new Rectangle
                        {
                            Fill = Brushes.White,
                            Width = 10,
                            Height = 10,
                            Margin = new Thickness(i * 10, j * 10, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        qrCode.Children.Add(whiteBlock);
                    }
                }
            }

            // 添加左上、右上、左下三个方形定位标记
            AddPositionMarker(qrCode, 0, 0);
            AddPositionMarker(qrCode, 50, 0);
            AddPositionMarker(qrCode, 0, 50);

            return new Border
            {
                Child = qrCode,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        // 添加QR码定位标记
        private void AddPositionMarker(Grid parent, double x, double y)
        {
            Rectangle outerRect = new Rectangle
            {
                Fill = Brushes.White,
                Width = 30,
                Height = 30,
                Margin = new Thickness(x, y, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            Rectangle innerRect = new Rectangle
            {
                Fill = Brushes.Black,
                Width = 20,
                Height = 20,
                Margin = new Thickness(x + 5, y + 5, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            Rectangle centerRect = new Rectangle
            {
                Fill = Brushes.White,
                Width = 10,
                Height = 10,
                Margin = new Thickness(x + 10, y + 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            parent.Children.Add(outerRect);
            parent.Children.Add(innerRect);
            parent.Children.Add(centerRect);
        }

        // 获取标签尺寸
        private Size GetLabelSize()
        {
            switch (SelectedSizeIndex)
            {
                case 0: // 小标签
                    return new Size(226, 151); // 60mm x 40mm (@ 96 DPI)
                case 2: // 大标签
                    return new Size(378, 302); // 100mm x 80mm (@ 96 DPI)
                default: // 中标签
                    return new Size(302, 226); // 80mm x 60mm (@ 96 DPI)
            }
        }

        // 打印预览
        [RelayCommand]
        private void PrintPreview()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // 创建要打印的文档
                    FixedDocument document = new FixedDocument();

                    // 创建页面内容
                    PageContent pageContent = new PageContent();
                    FixedPage fixedPage = new FixedPage();

                    // 获取标签尺寸
                    Size labelSize = GetLabelSize();
                    fixedPage.Width = labelSize.Width;
                    fixedPage.Height = labelSize.Height;

                    // 刷新标签内容确保最新
                    RefreshPreview();

                    // 添加标签内容到页面
                    UIElement labelContent = LabelPreview;
                    if (labelContent != null)
                    {
                        fixedPage.Children.Add(labelContent);
                    }

                    // 将页面添加到文档
                    ((IAddChild)pageContent).AddChild(fixedPage);
                    document.Pages.Add(pageContent);

                    // 显示打印预览
                    printDialog.PrintDocument(document.DocumentPaginator, "标签打印");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打印预览时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 直接打印
        [RelayCommand]
        private void PrintLabel()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // 刷新标签内容确保最新
                    RefreshPreview();

                    // 直接打印UI元素
                    UIElement labelContent = LabelPreview;
                    if (labelContent != null)
                    {
                        // 设置打印尺寸
                        Size labelSize = GetLabelSize();
                        labelContent.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        labelContent.Arrange(new Rect(0, 0, labelSize.Width, labelSize.Height));

                        // 打印
                        printDialog.PrintVisual(labelContent, "标签打印");

                        MessageBox.Show("标签打印已发送到打印机", "打印成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打印时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
