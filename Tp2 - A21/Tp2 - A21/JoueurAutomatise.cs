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
        /// La carte -1 de carreau si le bot pige une carte.<br/>
        /// La carte que le joueur veut jouer.
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
            return hasEight ??= new Carte(-1, Carte.Sorte.Carreau);
        }

        public Carte.Sorte ObtenirSortePouvoir8()
        {
            Carte sorteCarreau = new Carte(0, Carte.Sorte.Carreau);
            Carte sorteCoeur = new Carte(0, Carte.Sorte.Coeur);
            Carte sorteTrefle = new Carte(0, Carte.Sorte.Trèfle);
            Carte sortePique = new Carte(0, Carte.Sorte.Pique);
            foreach (Carte carte in Main)
            {
                if (carte.SorteCarte == Carte.Sorte.Carreau) sorteCarreau.Valeur++;
                if (carte.SorteCarte == Carte.Sorte.Coeur) sorteCoeur.Valeur++;
                if (carte.SorteCarte == Carte.Sorte.Trèfle) sorteTrefle.Valeur++; 
                if (carte.SorteCarte == Carte.Sorte.Pique) sortePique.Valeur++;
            }
            if (sorteCarreau.Valeur >= sorteCoeur.Valeur && sorteCarreau.Valeur >= sorteTrefle.Valeur && sorteCarreau.Valeur >= sortePique.Valeur)
            {
                Trace.WriteLine("Pouvoir 8 : Carreau");
                return Carte.Sorte.Carreau;
            }
            if (sorteCoeur.Valeur >= sorteTrefle.Valeur && sorteCoeur.Valeur >= sortePique.Valeur)
            {
                Trace.WriteLine("Pouvoir 8 : Coeur");
                return Carte.Sorte.Coeur;
            }
            if (sorteTrefle.Valeur >= sortePique.Valeur)
            {
                Trace.WriteLine("Pouvoir 8 : Trèfle");
                return Carte.Sorte.Trèfle;
            }
            Trace.WriteLine("Pouvoir 8 : Pique");
            return Carte.Sorte.Pique;
        }
    }
}
