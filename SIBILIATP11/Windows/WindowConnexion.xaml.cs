using SIBILIATP11.Classe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SIBILIATP11.Windows
{
    public partial class WindowConnexion : Window
    {
        private Dictionary<string, string> utilisateurs = new();
        private List<Employe> tousLesEmployes = new();
        public Employe EmployeConnecte { get; private set; }

        public WindowConnexion()
        {
            InitializeComponent();
            ChargerPasswordEtLogin();
            Loaded += (s, e) => TxtLogin.Focus();
        }

        private void ChargerPasswordEtLogin()
        {
            try
            {
                Employe e = new Employe();
                tousLesEmployes = e.FindAll();

                foreach (var employe in tousLesEmployes)
                {
                    employe.Read();
                }

                utilisateurs = tousLesEmployes.ToDictionary(emp => emp.Login, emp => emp.Password);
            }
            catch (Exception ex)
            {
                ShowError($"Erreur de connexion à la base de données");
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = $"❌ {message}";
            ErrorBorder.Visibility = Visibility.Visible;
        }

        private void SetErrorStyles()
        {
            var loginParent = TxtLogin.Parent as Border;
            if (loginParent != null)
            {
                loginParent.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6B6B"));
            }

            PasswordBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6B6B"));
        }

        private void ResetErrorStyles()
        {
            ErrorBorder.Visibility = Visibility.Collapsed;

            var loginParent = TxtLogin.Parent as Border;
            if (loginParent != null)
            {
                loginParent.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));
            }

            PasswordBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));
        }

        private void ButSeConnecter_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            /*string login = TxtLogin.Text.Trim();
            string mdp = TxtPassword.Password;

            ResetErrorStyles();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(mdp))
            {
                ShowError("Veuillez remplir tous les champs");
                return;
            }

            if (utilisateurs.ContainsKey(login) && utilisateurs[login] == mdp)
            {
                EmployeConnecte = tousLesEmployes.FirstOrDefault(emp => emp.Login == login);
                DialogResult = true;
                Close();
            }
            else
            {
                ShowError("Identifiant ou mot de passe incorrect");
                SetErrorStyles();
            }*/
        }

        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void TxtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButSeConnecter_Click(ButSeConnecter, new RoutedEventArgs());
            }
        }

        private void TxtLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            ResetErrorStyles();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ResetErrorStyles();
        }
    }
}