using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.UserInterface;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for AddRuleWindow_v2.xaml
    /// </summary>
    public partial class AddRuleWindow_v2 : Window {
        private static AddRuleWindow_v2 a;
        public List<Predicate<Slot>> PredicateList = new List<Predicate<Slot>>();

        private AddRuleWindow_v2() {
            InitializeComponent();
        }

        public static List<Predicate<Slot>> ShowWindow() {
            if (a == null) a = new AddRuleWindow_v2();
            a.ShowDialog();
            return a.PredicateList;
        }

        private void AddRuleButton_OnClick(object sender, RoutedEventArgs e) {
            ScrollViewer.ScrollToEnd();
            var r = new RuleSetter();
            r.xButton_Clicked += (o, args) => {
                var animation = CustomAnimation.GetLeavingScreenAnimation(r.ActualHeight, 0);
                animation.Completed += (sender1, eventArgs) => { innerSp.Children.Remove(o as RuleSetter); };
                r.BeginAnimation(HeightProperty, animation);
            };
            r.Loaded +=
                (o, args) => {
                    r.BeginAnimation(HeightProperty,
                        CustomAnimation.GetEnteringScreenAnimation(0, r.ActualHeight));
                };
            innerSp.Children.Add(r);
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e) {
            PredicateList.Clear();
            foreach (var child in innerSp.Children) {
                var r = child as RuleSetter;
                if (r == null) continue;
                if (!r.IsChecked) continue;
                PredicateList.Add(r.GetRulePredicate());
            }
            Hide();
        }

        private void AddRuleWindow_v2_OnClosing(object sender, CancelEventArgs e) {
            e.Cancel = true;
            Hide();
        }
    }
}