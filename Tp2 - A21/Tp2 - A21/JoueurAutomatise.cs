﻿using System.Linq;

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
    }
}
