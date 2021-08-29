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

namespace WeaponTester
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Weapon weapon = new Weapon(Int32.Parse(distInput.Text), Int32.Parse(accInput.Text), Int32.Parse(maxPenInput.Text), Int32.Parse(woundInput.Text));
            weapon.auto = autoInput.IsChecked.Value;
            weapon.optic = opticInput.IsChecked.Value;
            weapon.pool = poolInput.IsChecked.Value;
            weapon.power = powerInput.IsChecked.Value;
            weapon.up = upInput.IsChecked.Value;
            double damage = Math.Round(weapon.Shoot(Int32.Parse(skillInput.Text), Int32.Parse(armorInput.Text), Int32.Parse(distToEnemyInput.Text)), 2);
            avDamage.Text = damage.ToString();
        }
    }
}
