using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Tp2___A21
{
    public class MoteurDeJeu
    {

        private Stack<Carte> _lePaquetCartes;
        private Stack<Carte> _defausse;
        private Queue<Joueur> _lesJoueurs;

        private int _nbJoueurs;

        public Queue<Joueur> LesJoueurs
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
            LesJoueurs = new Queue<Joueur>();
            List<string> nomBots = new()
            {
                "Roboto",
                "Alexa",
                "Hal"
            };
            LesJoueurs.Enqueue(new Joueur(pNom));
            for (int i = 0; i < NbJoueurs-1; i++)
            {
                LesJoueurs.Enqueue(new JoueurAutomatise(nomBots[i]));
            }
            CreerJeuCartes();
            DistribuerCartes();
        }

        /// <summary>
        /// Cette méthode crée un jeu de 52 cartes sans les jokers.
        /// </summary>
        private void CreerJeuCartes()
        {
            _defausse = new Stack<Carte>();
            _lePaquetCartes = new Stack<Carte>();
            foreach (Carte.Sorte laSorte in Enum.GetValues(typeof(Carte.Sorte)))
            {
                for (int i = 2; i < 15; i++)
                {
                    _lePaquetCartes.Push(new Carte(i, laSorte));
                }
            }
            _lePaquetCartes = BrasserPaquet(_lePaquetCartes);
        }

        private Stack<Carte> BrasserPaquet(Stack<Carte> pLeStack)
        {
            List<Carte> cartes = pLeStack.ToList();

            cartes = cartes.OrderBy(_ => Guid.NewGuid()).ToList();
            Stack<Carte> stackBrasse = new();

            foreach (Carte carte in cartes)
            {
                stackBrasse.Push(carte);
            }

            return stackBrasse;
        }

        /// <summary>
        /// Cette méthode distribue les cartes aux joueurs.
        /// </summary>
        private void DistribuerCartes()
        {
            for (int i = 0; i < 8; i++)
            {
                foreach (Joueur joueur in LesJoueurs)
                {
                    joueur.Main.Add(_lePaquetCartes.Pop());
                }
            }

            _defausse.Push(_lePaquetCartes.Pop());

            //TODO: A experimenter:
            foreach (Joueur joueur in LesJoueurs)
            {
                joueur.Main = OrdonnerCartes(joueur.Main);
            }
        }

        public List<Carte> OrdonnerCartes(List<Carte> pCartes)
        {
            return pCartes
                .GroupBy(pCarte => pCarte.SorteCarte)
                .OrderBy(pCarte => pCarte.Key)
                .SelectMany(pGroup => pGroup
                    .OrderBy(pCarte => pCarte.Valeur))
                .ToList();
        }

        /// <summary>
        /// Cette méthode fait jouer les joueurs automatisés.
        /// </summary>
        /// <returns>
        /// -1 si le jeu continue.<br/>
        /// L'indice du joueur gagnant.
        /// </returns>
        public string FaireUnTour()
        {
            while (LesJoueurs.Peek() is JoueurAutomatise joueurAutomatise)
            {

                Carte carte = joueurAutomatise.JouerUnTour(ObtenirSommetDefausse());
                Trace.WriteLine($"Carte joué: {carte.Valeur} de { carte.SorteCarte} par {joueurAutomatise.Nom}");

                if (carte is {Valeur: -1, SorteCarte: Carte.Sorte.Carreau})
                {
                    PigerCarte(joueurAutomatise);
                }
                else
                {
                    if (joueurAutomatise.Main.Count == 0) return joueurAutomatise.Nom;
                    
                    LesJoueurs.Enqueue(LesJoueurs.Dequeue());

                    _defausse.Push(carte);

                    carte.ObtenirPouvoir(ref _lesJoueurs, _lePaquetCartes);
                }

            }
            Trace.WriteLine("Tour terminé.");
            return "";
        }

        private void GestionPaquetVide()
        {
            if (PaquetVide())
            {
                Stack<Carte> paquet = new Stack<Carte>();
                Carte sommet = _defausse.Pop();
                for (int j = 0; j < _defausse.Count; j++)
                {
                    paquet.Push(_defausse.Pop());
                }
                _defausse.Push(sommet);
                _lePaquetCartes = BrasserPaquet(paquet);
            }
        }

        /// <summary>
        /// Cette méthode joue la carte sélectionné par le joueur humain.
        /// </summary>
        /// <param name="pCarte">La carte à jouer.</param>
        public string JouerCarteHumain(Carte pCarte)
        {
            if (LesJoueurs.Peek().Main.Contains(pCarte))
            {
                _defausse.Push(pCarte);
                LesJoueurs.Peek().Main.Remove(pCarte);
                // Verifier si gagne
                if (LesJoueurs.Peek().Main.Count == 0) return LesJoueurs.Peek().Nom;

                LesJoueurs.Enqueue(LesJoueurs.Dequeue());

                pCarte.ObtenirPouvoir(ref _lesJoueurs, _lePaquetCartes);
            }
            string stringRetour = "";
            return stringRetour;
        }

        /// <summary>
        /// Cette méthode fait piger une carte au joueur actuel.
        /// </summary>
        public void PigerCarte(Joueur pJoueur)
        {
            pJoueur.Main.Add(_lePaquetCartes.Pop());
            pJoueur.Main = OrdonnerCartes(pJoueur.Main);
            GestionPaquetVide();

            LesJoueurs.Enqueue(LesJoueurs.Dequeue());
        }

        /// <summary>
        /// Cette méthode retourne la carte qui est au sommet
        /// de la défausse.
        /// </summary>
        /// <returns>La carte au sommet de la défausse</returns>
        public Carte ObtenirSommetDefausse()
        {
            return _defausse.Peek();
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