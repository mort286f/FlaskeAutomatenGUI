using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Media.Animation;

namespace FlaskeAutomatenGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Producer _producer = new Producer();
        Consumer _consumerOne = new Consumer("Hans");
        Consumer _consumerTwo = new Consumer("Grete");
        Splitter _splitter = new Splitter();
        List<Rectangle> consumedBottles = new List<Rectangle>();
        private bool running;
        int bottleSpeed = 6;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            running = true;
            Producer.isRunning = true;
            Consumer.isRunning = true;
            Splitter.isRunning = true;

            StartBackgroundThreads();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            running = false;
            Producer.isRunning = false;
            Consumer.isRunning = false;
            Splitter.isRunning = false;
        }

        private void UpdateUI()
        {
            while (running)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.producerCount.Text = Producer.prodQueue.Count.ToString();
                    this.ConsumerOneBufferCount.Text = Consumer.beerQueue.Count.ToString();
                    this.ConsumerTwoBufferCount.Text = Consumer.sodaQueue.Count.ToString();
                    this.producerBufferBar.Value = Producer.prodQueue.Count();
                    this.ConsumerOneBuffer.Value = Consumer.beerQueue.Count;
                    this.ConsumerTwoBuffer.Value = Consumer.sodaQueue.Count;
                    ShowProducedDrink();
                    ProduceDrinkProgressBar();
                });
                Thread.Sleep(50);
            }

        }

        private void MakeBottles()
        {
            ImageBrush bottleImage = new ImageBrush();

            if (Producer.isBeer)
            {
                bottleImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/BeerBottle.png"));
            }
            else if (!Producer.isBeer)
            {
                bottleImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/assets/SodaBottle.png"));
            }

            Rectangle newBottle = new Rectangle()
            {
                Tag = "bottle",
                Height = 40,
                Width = 15,
                Fill = bottleImage
            };

            Canvas.SetLeft(newBottle, 5);
            Canvas.SetBottom(newBottle, 5);

            this.BottleCanvas.Children.Add(newBottle);

            foreach (var bottle in BottleCanvas.Children.OfType<Rectangle>())
            {
                if ((string)bottle.Tag == "bottle")
                {
                    Canvas.SetTop(bottle, Canvas.GetTop(bottle) - bottleSpeed);
                }
                if (Canvas.GetRight(bottle) < 3)
                {
                    consumedBottles.Add(bottle);
                }
            }

            foreach (Rectangle bottle in consumedBottles)
            {
                BottleCanvas.Children.Remove(bottle);
            }
        }

        private void ProduceDrinkProgressBar()
        {
            this.Dispatcher.Invoke(() =>
            {

                this.ProducerProgressBar.Value += 500;
                if (this.ProducerProgressBar.Value == this.ProducerProgressBar.Maximum)
                {
                    this.ProducerProgressBar.Value = 0;
                }
            });
        }

        private void ShowProducedDrink()
        {
            if (Producer.isBeer)
            {
                this.BeerPanel.Visibility = Visibility.Visible;
                this.SodaPanel.Visibility = Visibility.Hidden;
            }
            else if (!Producer.isBeer)
            {
                this.BeerPanel.Visibility = Visibility.Hidden;
                this.SodaPanel.Visibility = Visibility.Visible;
            }
        }

        private void StartBackgroundThreads()
        {
            this.Dispatcher.Invoke(() =>
            {
                new Thread(() =>
                {
                    _producer.Produce();

                }).Start();

                new Thread(() =>
                {
                    _consumerOne.ConsumeBeer(_consumerOne.Name);

                }).Start();

                new Thread(() =>
                {
                    _consumerOne.ConsumeBeer(_consumerTwo.Name);

                }).Start();

                new Thread(() =>
                {
                    _splitter.SplitDrinks();

                }).Start();
            });
            this.Dispatcher.Invoke(() =>
            {
                new Thread(() =>
                {
                    UpdateUI();
                }).Start();
            });


        }

        private void producerBufferBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Producer.isAdded)
            {
                MakeBottles();
            }
        }
    }
}
