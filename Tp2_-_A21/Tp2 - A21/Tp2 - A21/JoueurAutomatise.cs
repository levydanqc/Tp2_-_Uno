using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp2___A21
{
    public class JoueurAutomatise : Joueur
    {
        public JoueurAutomatise(string pNom) : base(pNom)
        {
        }

        /// <summary>
        /// Cette méthode permet aux joueurs automatisé de jouer un tour
        /// </summary>
        /// <returns>La carte que le joueur veut jouer</returns>
        public Carte JouerUnTour()
        {
            Carte carteRetour = Main[0];
            Main.RemoveAt(0);
            return carteRetour;
        }
    }
}
