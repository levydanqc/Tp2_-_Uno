using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp2___A21
{
    public class MoteurDeJeu
    {
        private List<Carte> _lePaquetCartes;
        private List<Carte> _defausse;
        private List<Joueur> _lesJoueurs;
        private int _nbJoueurs;

        public List<Joueur> LesJoueurs
        {
            get { return _lesJoueurs; }
            set { _lesJoueurs = value; }
        }

        public int NbJoueurs
        {
            get { return _nbJoueurs; }
            set { _nbJoueurs = value; }
        }

        /// <summary>
        /// Constructeur de la classe. La liste de joueurs est initialisée.
        /// Puis, le paquet de cartes est créé et distribué.
        /// </summary>
        public MoteurDeJeu(int pNbJoueurs, string pNom)
        {
            NbJoueurs = pNbJoueurs;
            LesJoueurs = new List<Joueur>();
            //LesJoueurs.Add(new Joueur("Bob"));
            //LesJoueurs.Add(new JoueurAutomatise("Roboto"));
            //LesJoueurs.Add(new JoueurAutomatise("Alexa"));
            //LesJoueurs.Add(new JoueurAutomatise("Hal"));
            for (int i = 0; i < NbJoueurs; i++)
            {
                if (i == 0)
                    LesJoueurs.Add(new Joueur(pNom));
                else if (i == 1)
                    LesJoueurs.Add(new JoueurAutomatise("Roboto"));
                else if (i == 2)
                    LesJoueurs.Add(new JoueurAutomatise("Alexa"));
                else if (i == 3)
                    LesJoueurs.Add(new JoueurAutomatise("Hal"));
            }
            CreerJeuCartes();
            DistribuerCartes();
        }

        /// <summary>
        /// Cette méthode crée un jeu de 52 cartes sans les jokers.
        /// </summary>
        private void CreerJeuCartes()
        {
            _defausse = new List<Carte>();
            _lePaquetCartes = new List<Carte>();
            foreach (Carte.Sorte laSorte in Enum.GetValues(typeof(Carte.Sorte)))
            {
                for (int i = 2; i < 15; i++)
                {
                    _lePaquetCartes.Add(new Carte(i, laSorte));
                }
            }
        }

        /// <summary>
        /// Cette méthode distribue les cartes aux joueurs.
        /// </summary>
        private void DistribuerCartes()
        {
            int j = 0;
            for (int i = 0; i < 8; i++)
            {
                foreach (Joueur joueur in LesJoueurs)
                {
                    j = Utilitaires.aleatoire.Next(_lePaquetCartes.Count - 1);
                    joueur.Main.Add(_lePaquetCartes[j]);
                    _lePaquetCartes.RemoveAt(j);
                }
            }
        }


        /// <summary>
        /// Cette méthode fait jouer les joueurs automatisés.
        /// </summary>
        public void FaireUnTour()
        {
            for (int i = 1; i < NbJoueurs; i++)
            {
                _defausse.Add(((JoueurAutomatise) LesJoueurs[i]).JouerUnTour());
            }
        }

        /// <summary>
        /// Cette méthode joue la carte sélectionné par le joueur humain.
        /// </summary>
        /// <param name="pIndiceCarte"></param>
        public void JouerCarteHumain(int pIndiceCarte)
        {
            if (pIndiceCarte < LesJoueurs[0].Main.Count && pIndiceCarte > -1)
            {
                _defausse.Add(LesJoueurs[0].Main[pIndiceCarte]);
                LesJoueurs[0].Main.RemoveAt(pIndiceCarte);
            }
        }

        /// <summary>
        /// Cette méthode fait piger une carte au joueur humain.
        /// </summary>
        public void PigerCarteHumain()
        {
            LesJoueurs[0].Main.Add(_lePaquetCartes[0]);
            _lePaquetCartes.RemoveAt(0);
        }

        /// <summary>
        /// Cette méthode retourne la carte qui est au sommet
        /// de la défausse.
        /// </summary>
        /// <returns>La carte au sommet de la défausse</returns>
        public Carte ObtenirSommetDefausse()
        {
            return _defausse[^1];
        }

        /// <summary>
        /// Cette méthode retourne un bool qui permet de savoir
        /// si la défausse est vide.
        /// </summary>
        /// <returns>True si la défausse est vide, faux sinon</returns>
        public bool DefausseVide()
        {
            return _defausse.Count == 0;
        }

        /// <summary>
        /// Cette méthode retourne un bool qui permet de savoir
        /// si le paquet est vide.
        /// </summary>
        /// <returns>True si le paquet est vide, faux sinon</returns>
        public bool PaquetVide()
        {
            return _lePaquetCartes.Count == 0;
        }
    }
}