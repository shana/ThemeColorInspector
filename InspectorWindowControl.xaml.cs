using System;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;

using System.Collections.Generic;
using SpoiledCat.Utils.Colors;

namespace ThemeColorInspector
{

    /// <summary>
    /// Interaction logic for InspectorWindowControl.
    /// </summary>
    public partial class InspectorWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InspectorWindowControl"/> class.
        /// </summary>
        public InspectorWindowControl()
        {
            this.InitializeComponent();

            FillColors();
            VSColorTheme.ThemeChanged += _ => FillColors();
        }

        SortedDictionary<string, Brush> brushResources = new SortedDictionary<string, Brush>();
        void FillColors()
        {
            container.Children.Clear();
            brushResources.Clear();
            var props = typeof(VsBrushes).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var prop in props.Where(x => x.Name.EndsWith("Key")))
            {
                var name = prop.Name.Substring(0, prop.Name.Length - 3);
                if (!brushResources.ContainsKey(name))
                    brushResources.Add(name, (Brush)FindResource("VsBrush." + name));
            }

            foreach (var kvp in brushResources)
            {
                var name = kvp.Key;
                var brush = kvp.Value;

                var lbl = new Label();
                lbl.Background = brush;
                var c = (Color)new BrushToColorConverter().Convert(lbl.Background, typeof(Color), null, null);
                lbl.Content = name + " " + c.R + " " + c.G + " " + c.B + " " + c.ToLowercaseString();
                lbl.BorderBrush = Brushes.DarkRed;
                if (c.IsDark())
                    lbl.Foreground = Brushes.White;
                else
                    lbl.Foreground = Brushes.Black;
                container.Children.Add(lbl);
            }
        }

        List<Label> searchResults = new List<Label>();
        int currentResult = -1;

        void search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var txt = search.Text;
                Color color;
                if (!SetupSearch(txt, out color)) return;

                searchResults.AddRange(container.Children.Cast<UIElement>()
                    .Select(x => x as Label)
                    .Where(x => x != null)
                    .Where(x => color == (Color)new BrushToColorConverter().Convert(x.Background, typeof(Color), null, null)));

                if (searchResults.Count == 0)
                {
                    MessageBox.Show("No colors found that match {0}", txt);
                    return;
                }

                GotoNextLabel();

            }
        }

        void searchSimilar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var txt = searchSimilar.Text;
                Color color;
                if (!SetupSearch(txt, out color)) return;

                double fuzziness = 15;
                searchResults.AddRange(container.Children.Cast<UIElement>()
                    .Select(x => x as Label)
                    .Where(x => x != null)
                    .Where(x => color.Distance((Color)new BrushToColorConverter().Convert(x.Background, typeof(Color), null, null)) < fuzziness));

                if (searchResults.Count == 0)
                {
                    MessageBox.Show("No similar colors found");
                    return;
                }

                GotoNextLabel();
            }
        }

        bool SetupSearch(string txt, out Color color)
        {
            color = Colors.Black;
            if (string.IsNullOrWhiteSpace(txt))
                return false;
            var parts = txt.Split(' ');
            if (parts.Length < 3)
            {
                MessageBox.Show("Insert a color in 'R G B' format (spaces required)");
                return false;
            }
            byte r, g, b;
            if (!byte.TryParse(parts[0], out r) ||
                !byte.TryParse(parts[1], out g) ||
                !byte.TryParse(parts[2], out b))
            {
                MessageBox.Show("Invalid rgb values");
                return false;
            }
            if (currentResult >= 0)
                searchResults[currentResult].BorderThickness = new Thickness(0);
            searchResults.Clear();
            currentResult = -1;
            color = Color.FromRgb(r, g, b);
            return true;
        }

        Label GotoNextLabel()
        {
            return GotoLabel(1);
        }

        Label GotoPreviousLabel()
        {
            return GotoLabel(-1);
        }

        Label GotoLabel(int direction)
        {
            if (currentResult >= 0)
            {
                searchResults[currentResult].BorderThickness = new Thickness(0);
                currentResult += direction;
            }
            else
                currentResult = direction < 0 ? searchResults.Count - 1 : 0;
            
            if (currentResult == searchResults.Count)
                currentResult = 0;
            else if (currentResult < 0)
                currentResult = searchResults.Count - 1;
            var lbl = searchResults[currentResult];
            lbl.BringIntoView();
            lbl.BorderThickness = new Thickness(2);
            return lbl;
        }

        void prev_Click(object sender, RoutedEventArgs e)
        {
            if (searchResults.Count == 0)
                return;
            GotoPreviousLabel();
        }

        void next_Click(object sender, RoutedEventArgs e)
        {
            if (searchResults.Count == 0)
                return;
            GotoNextLabel();
        }
    }
}