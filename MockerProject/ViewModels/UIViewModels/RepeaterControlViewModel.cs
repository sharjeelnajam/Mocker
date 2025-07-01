using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using MockerProject.Views;
using MockerProject.Views.UIControls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MockerProject.ViewModels.UIViewModels
{
    internal class RepeaterControlViewModel : UIControlViewModel
    {
        public ReactiveCommand<Unit, Unit> AddItems { get; }
        public ReactiveCommand<Unit, Unit> InsertAfter { get; }

        public ReactiveCommand<Unit, Unit> InsertBefore { get; }

        public RepeaterControlViewModel(UIControl uiControl) : base(uiControl)
        {

            m_UIControl = uiControl;

            
           


            AddItems = ReactiveCommand.Create(ExecuteAddItems);
            InsertAfter = ReactiveCommand.Create(InsertAfterItems);
            InsertBefore = ReactiveCommand.Create(InsertBeforeItems);
           


        }
        private void InsertAfterItems()
        {

           
            // Handle the click event logic here
        }

        private void InsertBeforeItems()
        {

          
            // Handle the click event logic here
        }

        private void ExecuteAddItems()
        {
            
            ContainerBoxControl containerBoxControl = new ContainerBoxControl();
            containerBoxControl.setMainVM( this.m_MainVM);
            containerBoxControl.m_nUIControlType = Models.CONTROL_TYPE.CONTAINERBOX;
            //containerBoxControl.container.Children.Add(new ButtonControl());
            Items.Add(containerBoxControl);
            TabHeaders.Add("Item " + Items.Count);
            if (m_UIControl.m_nUIControlType == Models.CONTROL_TYPE.TABS)
            {

                int index = Items.Count - 1;
                //ListBoxItem listBoxItem = (ListBoxItem)((DropDownControl)w_UIControl).listBox.ContainerFromIndex(index);
                TabItem item = (TabItem)((TabViewControl)m_UIControl).tabControl.ContainerFromIndex(index);
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Item" + Items.Count;
                textBlock.Foreground = new SolidColorBrush(new Color(255, 0, 0, 0));


                item.Header = textBlock;



            }
        }

        public ObservableCollection<ContainerBoxControl> _Items = new ObservableCollection<ContainerBoxControl>();
        public ObservableCollection<ContainerBoxControl> Items
        {
            get { return _Items; }
            set { this.RaiseAndSetIfChanged(ref _Items, value); }
        }

        public ObservableCollection<string> _TabHeaders = new ObservableCollection<string>();
        public ObservableCollection<string> TabHeaders
        {
            get { return _TabHeaders; }
            set { this.RaiseAndSetIfChanged(ref _TabHeaders, value); }
        }
    }

}
