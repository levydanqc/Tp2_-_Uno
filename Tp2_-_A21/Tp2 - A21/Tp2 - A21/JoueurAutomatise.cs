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
        /// -1 si le bot pige une carte.
        /// La carte que le joueur veut jouer
        /// </returns>
        public Carte JouerUnTour(Carte pSommet)
        {
            foreach (Carte carte in Utilitaires.Aleatoire.Next(100) > 50 ? Main : Enumerable.Reverse(Main).ToList())
            {
                if (pSommet.Valeur == carte.Valeur || pSommet.SorteCarte == carte.SorteCarte)
                {
                    Main.Remove(carte);
                    return carte;
                }
            }
            return new Carte(-1, Carte.Sorte.Carreau);
        }
    }
}
