﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Tp2___A21
{
    public class MoteurDeJeu
    {
        //private List<Carte> _lePaquetCartes;
        //private List<Carte> _defausse;
        //private List<Joueur> _lesJoueurs;

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
            for (int i = 0; i < NbJoueurs; i++)
            {
                switch (i)
                {
                    case 1:
                        LesJoueurs.Enqueue(new JoueurAutomatise("Roboto"));
                        break;
                    case 2:
                        LesJoueurs.Enqueue(new JoueurAutomatise("Alexa"));
                        break;
                    case 3:
                        LesJoueurs.Enqueue(new JoueurAutomatise("Hal"));
                        break;
                    default:
                        LesJoueurs.Enqueue(new Joueur(pNom));
                        break;
                }

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
            BrasserPaquet(_lePaquetCartes);
        }

        private void BrasserPaquet(Stack<Carte> leStack)
        {
            // TODO
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
                    //_lePaquetCartes.RemoveAt(0);
                }
            }

            _defausse.Push(_lePaquetCartes.Pop());
            //_lePaquetCartes.RemoveAt(0);

            //A experimenter:
            foreach (Joueur joueur in LesJoueurs)
            {
                joueur.Main = OrdonnerCartes(joueur.Main);
            }
        }

        public List<Carte> OrdonnerCartes(List<Carte> cartes)
        {
            cartes.GroupBy(l => l.SorteCarte).OrderByDescending(g => g.Count()).SelectMany(g => g.OrderBy(c => c.Valeur));
            //var sorted = cartes
            //    .GroupBy(l => l.SorteCarte)
            //    .OrderBy(g => g.Count())
            //    .SelectMany(g => g.OrderBy(c => c.Valeur));
            //return sorted as List<Carte>;
            return cartes;
        }

        /// <summary>
        /// Cette méthode fait jouer les joueurs automatisés.
        /// </summary>
        /// <returns>
        /// -1 si le jeu continue.
        /// L'indice du joueur gagnant.
        /// </returns>
        public int FaireUnTour()
        {
            //for (int i = 1; i < NbJoueurs; i++)
            //{
            //    Joueur leJoueur = LesJoueurs.Peek() as JoueurAutomatise;
            //    Carte carte = (LesJoueurs.Peek() as JoueurAutomatise)?.JouerUnTour(ObtenirSommetDefausse());

            //    if (carte is {Valeur: -1})
            //    {
            //        (LesJoueurs.Peek() as JoueurAutomatise)?.Main.Add(_lePaquetCartes.Pop());
            //        //_lePaquetCartes.RemoveAt(0);
            //    }
            //    else
            //    {
            //        _defausse.Push(carte);
            //    }

            //    if (LesJoueurs.Peek().Main.Count == 0)
            //    {
            //        return i;
            //    }
            //}
            int i = 0;
            foreach (Joueur joueur in LesJoueurs)
            {
                if (joueur is JoueurAutomatise)
                {
                    i++;
                    Carte carte = (joueur as JoueurAutomatise).JouerUnTour(ObtenirSommetDefausse());
                    if (carte is { Valeur: -1 })
                    {
                        joueur.Main.Add(_lePaquetCartes.Pop());
                    }
                    else
                    {
                        _defausse.Push(carte);
                    }

                    if (joueur.Main.Count == 0)
                        return i;
                }

            }

            return -1;
        }

        /// <summary>
        /// Cette méthode joue la carte sélectionné par le joueur humain.
        /// </summary>
        /// <param name="pIndiceCarte"></param>
        public void JouerCarteHumain(int pIndiceCarte)
        {
            if (pIndiceCarte < LesJoueurs.Peek().Main.Count && pIndiceCarte > -1)
            {
                _defausse.Push(LesJoueurs.Peek().Main[pIndiceCarte]);
                LesJoueurs.Peek().Main.RemoveAt(pIndiceCarte);
            }
        }

        /// <summary>
        /// Cette méthode fait piger une carte au joueur humain.
        /// </summary>
        public void PigerCarteHumain()
        {
            LesJoueurs.Peek().Main.Add(_lePaquetCartes.Pop());
            //_lePaquetCartes.RemoveAt(0);
        }

        /// <summary>
        /// Cette méthode retourne la carte qui est au sommet
        /// de la défausse.
        /// </summary>
        /// <returns>La carte au sommet de la défausse</returns>
        public Carte ObtenirSommetDefausse()
        {
            //return _defausse[^1];
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