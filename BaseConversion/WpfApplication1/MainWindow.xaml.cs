using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace BaseConversion {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void buttonConvert_Click(object sender, RoutedEventArgs e) {
            //according to the drop down list, convert textBoxInput into textBoxOutput
            String inputString = textBoxInput.Text.ToString();

            //get from and to base
            int fromBase = Convert.ToInt32(comboBoxFrom.Text.ToString());
            int toBase = Convert.ToInt32(comboBoxTo.Text.ToString());

            //clean string and validate
            CleanString(inputString);

            if (ValidateInputWithBaseSelection(inputString, fromBase)) {
                labelOutput.Content = "Number entered does not match with \"From Base\" Selected, please select again";
            } else {
                StringBuilder outputString = BaseConvert(inputString, fromBase, toBase);
                labelOutput.Content = outputString;
            }

        }//end buttonConvert_Click

        public void CleanString(String s) {
            if (s.Length != 0) {
                s.Replace(" ", "");
                while (s.Length < 8) {
                    s.Insert(0, "0");
                }
            }
        }//end CleanString

        public Boolean ValidateInputWithBaseSelection(String s, int frBase) {

            //check if the base selected is the same as the number inputed
            int input = Convert.ToInt32(s);
            Regex alphabetRegex = new Regex("([0-9]+[a-f])([0-9]+[a-f])([0-9]+[a-f])([0-9]+[a-f])([0-9]+[a-f])([0-9]+[a-f])([0-9]+[a-f])([0-9]+[a-f])", RegexOptions.IgnoreCase);
            MatchCollection matches = alphabetRegex.Matches(s);

            switch (frBase) {
                case 2:
                    if (s.IndexOf('3') != -1
                        || s.IndexOf('4') != -1
                        || s.IndexOf('5') != -1
                        || s.IndexOf('6') != -1
                        || s.IndexOf('7') != -1
                        || s.IndexOf('8') != -1
                        || s.IndexOf('9') != -1
                        || s.IndexOf('A') != -1
                        || s.IndexOf('B') != -1
                        || s.IndexOf('C') != -1
                        || s.IndexOf('D') != -1
                        || s.IndexOf('E') != -1
                        || s.IndexOf('F') != -1) {
                        labelOutput.Content = "Invalid Number";
                    }
                    break;
                case 8:
                    if (s.IndexOf('8') != -1
                        || s.IndexOf('9') != -1) {
                        labelOutput.Content = "Invalid Number";
                    }
                    break;
                case 10:
                    if (s.IndexOf('A') != -1
                        || s.IndexOf('B') != -1
                        || s.IndexOf('C') != -1
                        || s.IndexOf('D') != -1
                        || s.IndexOf('E') != -1
                        || s.IndexOf('F') != -1) {
                        labelOutput.Content = "Invalid Number";
                    }
                    break;
                case 16:
                    break;
                default:
                    break;
            }
            return true;
        }//end ValidateInputWithBaseSelection

        public StringBuilder BaseConvert(String s, int fromBase, int toBase) {
            int dec = 0;
            StringBuilder resultS = new StringBuilder("");
            switch (fromBase) {
                case 2:
                case 8:
                case 16:
                    dec = ConvertAnyBaseToDecimal(s, fromBase);
                    resultS = ConvertDecimalToAnyBase(dec, toBase);
                    break;
                case 10:
                    dec = Convert.ToInt32(s);
                    resultS = ConvertDecimalToAnyBase(dec, toBase);
                    break;

                default:
                    resultS.Append("Program only can perform base conversion between 2, 8, 10 and 16");
                    break;
            }// end switch

            return resultS;

        }// end baseConvert

        public int ConvertAnyBaseToDecimal(String s, int fromBase) {
            int result = 0;
            int basePower = 1;

            if (fromBase != 16) {
                for (int i = s.Length - 1; i >= 0; i--) {
                    result += (s[i] - '0') * basePower;
                    basePower *= fromBase;
                }
            } else {
                for (int i = s.Length - 1; i >= 0; i--) {
                    int x = s[i] - '0';
                    if (x < 10) {
                        result += x * basePower;
                    } else {
                        switch (s[i]) {
                            case 'A':
                                result += 10 * basePower;
                                break;
                            case 'B':
                                result += 11 * basePower;
                                break;
                            case 'C':
                                result += 12 * basePower;
                                break;
                            case 'D':
                                result += 13 * basePower;
                                break;
                            case 'E':
                                result += 14 * basePower;
                                break;
                            case 'F':
                                result += 15 * basePower;
                                break;
                        }
                    }
                    basePower *= fromBase;
                }
            }

            return result;
        }// end anyToDec

        public StringBuilder ConvertDecimalToAnyBase(int dec, int toBase) {
            StringBuilder resultS = new StringBuilder("");

            while (dec > 0) {
                int x = (dec % toBase);
                if (x >= 10) {
                    switch (x) {
                        case 10:
                            resultS.Insert(0, 'A');
                            break;
                        case 11:
                            resultS.Insert(0, 'B');
                            break;
                        case 12:
                            resultS.Insert(0, 'C');
                            break;
                        case 13:
                            resultS.Insert(0, 'D');
                            break;
                        case 14:
                            resultS.Insert(0, 'E');
                            break;
                        case 15:
                            resultS.Insert(0, 'F');
                            break;
                    }
                } else {
                    resultS.Insert(0, x);
                }

                dec /= toBase;
            }

            return resultS;
        }

        private void textBoxInput_TextChanged(object sender, TextChangedEventArgs e) {
            if (textBoxInput.Text.ToString() == "") {
                labelOutput.Content = "";
            }
        }

        private void textBoxInput_GotFocus(object sender, RoutedEventArgs e) {
            textBoxInput.Clear();
            labelOutput.Content = "";
        }

        private void comboBoxFrom_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBoxItem cbi = (ComboBoxItem)comboBoxFrom.SelectedItem;
            String s = cbi.Content.ToString();
            comboBoxTo.Items.Clear();
            switch (s) {
                case "2":
                    comboBoxTo.Items.Add(8);
                    comboBoxTo.Items.Add(10);
                    comboBoxTo.Items.Add(16);
                    break;
                case "8":
                    comboBoxTo.Items.Add(2);
                    comboBoxTo.Items.Add(10);
                    comboBoxTo.Items.Add(16);
                    break;
                case "10":
                    comboBoxTo.Items.Add(2);
                    comboBoxTo.Items.Add(8);
                    comboBoxTo.Items.Add(16);
                    break;
                case "16":
                    comboBoxTo.Items.Add(2);
                    comboBoxTo.Items.Add(8);
                    comboBoxTo.Items.Add(10);
                    break;
            }
            comboBoxTo.Focus();
        }

        private void comboBoxTo_GotFocus(object sender, RoutedEventArgs e) {

        }// end decToAny

    }
}
