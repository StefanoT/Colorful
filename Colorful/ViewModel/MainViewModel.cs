/*
Project: Colorful
Author: Stefano Tommesani
Website: http://www.tommesani.com
Microsoft Public License (MS-PL) [OSI Approved License]
This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.
2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
*/

using GalaSoft.MvvmLight;
using Colorful.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Threading.Tasks;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;

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

        private readonly IDialogService _dialogService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IColorService colorService, IDialogService dialogService)
        {
            _colorService = colorService;
            _dialogService = dialogService;

            PickImageCommand = new RelayCommand(
                PickImage);
        }

        /// <summary>
        /// run measure computation in a separate task, then continue with the UI update with results
        /// </summary>
        private void ComputeColorfulness()
        {            
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
            var settings = new OpenFileDialogSettings
            {
                Title = "Pick an image",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Filter = "Jpeg images (*.jpg)|*.jpg;*.jpeg|All Files (*.*)|*.*"
            };

            bool? success = _dialogService.ShowOpenFileDialog(this, settings);
            if (success == true)
            {
                ColorIndex = null;
                ImageFileName = settings.FileName;                               
                ComputeColorfulness();
            }
        }
    }
}