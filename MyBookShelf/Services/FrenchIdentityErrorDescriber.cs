using Microsoft.AspNetCore.Identity;

namespace MyBookShelf.Services
{
    /// <summary>
    /// Remplace les messages d'erreur par defaut d'ASP.NET Core Identity par des
    /// messages en francais, comprehensibles et sans termes techniques.
    /// </summary>
    public class FrenchIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
            => new() { Code = nameof(DefaultError), Description = "Une erreur inattendue s'est produite." };

        public override IdentityError ConcurrencyFailure()
            => new() { Code = nameof(ConcurrencyFailure), Description = "Les informations ont ete modifiees par ailleurs. Veuillez reessayer." };

        public override IdentityError PasswordMismatch()
            => new() { Code = nameof(PasswordMismatch), Description = "Le mot de passe actuel est incorrect." };

        public override IdentityError InvalidToken()
            => new() { Code = nameof(InvalidToken), Description = "Le lien de validation n'est plus valide. Veuillez reessayer." };

        public override IdentityError RecoveryCodeRedemptionFailed()
            => new() { Code = nameof(RecoveryCodeRedemptionFailed), Description = "Ce code de recuperation a deja ete utilise." };

        public override IdentityError LoginAlreadyAssociated()
            => new() { Code = nameof(LoginAlreadyAssociated), Description = "Cette connexion est deja associee a un compte." };

        public override IdentityError InvalidUserName(string? userName)
            => new() { Code = nameof(InvalidUserName), Description = $"Le nom d'utilisateur {userName} n'est pas valide. Il ne peut contenir que des lettres et des chiffres." };

        public override IdentityError InvalidEmail(string? email)
            => new() { Code = nameof(InvalidEmail), Description = $"L'adresse e-mail {email} n'est pas valide." };

        public override IdentityError DuplicateUserName(string? userName)
            => new() { Code = nameof(DuplicateUserName), Description = "Ce nom d'utilisateur est deja utilise." };

        public override IdentityError DuplicateEmail(string? email)
            => new() { Code = nameof(DuplicateEmail), Description = "Un compte existe deja avec cette adresse e-mail." };

        public override IdentityError InvalidRoleName(string? role)
            => new() { Code = nameof(InvalidRoleName), Description = "Ce nom de role n'est pas valide." };

        public override IdentityError DuplicateRoleName(string? role)
            => new() { Code = nameof(DuplicateRoleName), Description = "Ce role existe deja." };

        public override IdentityError UserAlreadyHasPassword()
            => new() { Code = nameof(UserAlreadyHasPassword), Description = "Vous avez deja defini un mot de passe." };

        public override IdentityError UserLockoutNotEnabled()
            => new() { Code = nameof(UserLockoutNotEnabled), Description = "La protection contre les tentatives repetees n'est pas activee pour ce compte." };

        public override IdentityError UserAlreadyInRole(string? role)
            => new() { Code = nameof(UserAlreadyInRole), Description = "Cet utilisateur est deja dans ce role." };

        public override IdentityError UserNotInRole(string? role)
            => new() { Code = nameof(UserNotInRole), Description = "Cet utilisateur n'est pas dans ce role." };

        public override IdentityError PasswordTooShort(int length)
            => new() { Code = nameof(PasswordTooShort), Description = $"Le mot de passe doit contenir au moins {length} caracteres." };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => new() { Code = nameof(PasswordRequiresUniqueChars), Description = $"Le mot de passe doit utiliser au moins {uniqueChars} caracteres differents." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Le mot de passe doit contenir au moins un caractere special (par exemple !, ?, #, -)." };

        public override IdentityError PasswordRequiresDigit()
            => new() { Code = nameof(PasswordRequiresDigit), Description = "Le mot de passe doit contenir au moins un chiffre." };

        public override IdentityError PasswordRequiresLower()
            => new() { Code = nameof(PasswordRequiresLower), Description = "Le mot de passe doit contenir au moins une lettre minuscule." };

        public override IdentityError PasswordRequiresUpper()
            => new() { Code = nameof(PasswordRequiresUpper), Description = "Le mot de passe doit contenir au moins une lettre majuscule." };
    }
}
