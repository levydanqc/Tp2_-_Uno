using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        /// <returns>
        /// La carte que le joueur veut jouer.<br/>
        /// Null si le joueur pioche.
        /// </returns>
        public Carte JouerUnTour(Carte pSommet)
        {
            Carte? hasEight = null;
            foreach (Carte carte in Utilitaires.Aleatoire.Next(100) > 50 ? Main : Main.Reverse())
            {
                if (carte.Valeur == 8) hasEight = carte;
                if (pSommet.Valeur != carte.Valeur && pSommet.SorteCarte != carte.SorteCarte) continue;
                Main.Remove(carte);
                return carte;
            }
            Main.Remove(hasEight);
            return hasEight;
        }

        #region Overrides of Joueur

        public override Carte.Sorte ObtenirSortePouvoir8()
        {
            int[] nbCouleurs = new int[4];

            foreach (Carte carte in Main)
            {
                nbCouleurs[(int)carte.SorteCarte]++;
            }

            int max = nbCouleurs.Max();
            int i = nbCouleurs.ToList().IndexOf(max);

            return (Carte.Sorte)i;
        }

        #endregion
    }
}
