using GalaSoft.MvvmLight;
using Colorful.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Threading.Tasks;

namespace Colorful.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IColorService _colorService;

        private string _imageFileName = string.Empty;
        private double? _colorIndex = null;

        /// <summary>
        /// Gets the imageFileName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ImageFileName
        {
            get
            {
                return _imageFileName;
            }
            set
            {
                Set(ref _imageFileName, value);
            }
        }

        /// <summary>
        /// Gets the imageFileName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double? ColorIndex
        {
            get
            {
                return _colorIndex;
            }
            set
            {
                Set(ref _colorIndex, value);
            }
        }

        public RelayCommand PickImageCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IColorService colorService)
        {
            _colorService = colorService;

            PickImageCommand = new RelayCommand(
                PickImage);
        }

        private void ComputeColorfulness()
        {
            // run measure computation in a separate task, then continue with the UI update with results
            Task computeTask = Task.Factory.StartNew(() => _colorService.ComputeColorIndex(ImageFileName)).ContinueWith((t) =>
            {
                ColorIndex = t.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// let the user select an image and then compute the colorfulness measure
        /// </summary>
        private void PickImage()
        {
            // create the OpenFileDialog object
            Microsoft.Win32.OpenFileDialog openPicker = new Microsoft.Win32.OpenFileDialog();
            // Add file filters           
            openPicker.DefaultExt = ".jpg";
            openPicker.Filter = "Image Files|*.jpg;*.jpeg";
            // display the OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openPicker.ShowDialog();
            // check to see if we have a result 
            if (result == true)
            {
                ColorIndex = null;
                ImageFileName = openPicker.FileName.ToString();
                ComputeColorfulness();
            }
        }
    }
}