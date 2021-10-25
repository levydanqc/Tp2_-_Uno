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
using System.Windows.Shapes;

namespace Tp2___A21
{
    /// <summary>
    /// Logique d'interaction pour FormChoixCouleur.xaml
    /// </summary>
    public partial class FormChoixCouleur : Window
    {
        private Carte.Sorte? _couleur = null;

        public Carte.Sorte? Couleur
        {
            get { return _couleur; }
            set { _couleur = value; }
        }


        public FormChoixCouleur()
        {
            InitializeComponent();
        }

        private void btnChoixCoeur_Click(object sender, RoutedEventArgs e)
        {
            Couleur = Carte.Sorte.Coeur;
            this.Close();
        }

        private void btnChoixCarreau_Click(object sender, RoutedEventArgs e)
        {
            Couleur = Carte.Sorte.Carreau;
            this.Close();
        }

        private void btnChoixTrefle_Click(object sender, RoutedEventArgs e)
        {
            Couleur = Carte.Sorte.Trèfle;
            this.Close();
        }

        private void btnChoixPique_Click(object sender, RoutedEventArgs e)
        {
            Couleur = Carte.Sorte.Pique;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Couleur is null)
            {
                e.Cancel = true;
            }

            // TODO : Show error label (no button checked)
        }

        // TODO: Change label color when selected
    }
}
