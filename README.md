
#  P7CreateRestApi

API REST sécurisée développée en **ASP.NET Core** avec authentification JWT, gestion des utilisateurs via Identity et tests unitaires.

---

##  Objectif

Concevoir une API RESTful complète respectant les bonnes pratiques :

- CRUD sur plusieurs entités
- Authentification sécurisée (JWT)
- Autorisation par rôles
- Validation des données
- Logs applicatifs
- Tests unitaires

---

##  Stack technique

- **C# / .NET 7**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server (LocalDB)**
- **ASP.NET Identity**
- **JWT Authentication**
- **xUnit (tests)**

---

##  Fonctionnalités

-  Authentification JWT
-  Gestion des utilisateurs (Identity)
-  CRUD complet :
  - BidList
  - CurvePoint
  - Rating
  - RuleName
  - Trade
-  Logs sur tous les endpoints
-  Tests unitaires sur les repositories

---

##  Sécurité

- Endpoints protégés avec [Authorize]
- Gestion des rôles (Admin)
- Validation des entrées utilisateur
