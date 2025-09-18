# Provisionnement métier d'une salle de formation Azure

## Objectif métier

Permettre à chaque stagiaire d'une salle de formation d'obtenir une machine virtuelle dédiée dans Azure, avec une adresse IP publique pour se connecter à distance.

## Processus

- Lors de la création d'une salle de formation, le système provisionne automatiquement une VM pour chaque stagiaire.
- Chaque VM est accessible via une IP publique, transmise au stagiaire pour sa session.
- Toutes les informations de provisionnement sont historisées pour traçabilité et support.

## Bénéfices

- Automatisation du déploiement des environnements de formation.
- Attribution claire des ressources à chaque stagiaire.
- Suivi et audit des accès et des ressources provisionnées.

## Données stockées

- Pour chaque stagiaire : salle, VM, IP publique, date de provisionnement.
- Ces données sont utilisées pour l'affichage, la gestion et la facturation.

---

**Résumé** :  
Ce processus garantit que chaque stagiaire dispose d'un environnement isolé, traçable et prêt à l'emploi pour la formation, tout en permettant à l'organisation de piloter et d'auditer l'usage des ressources Azure.
