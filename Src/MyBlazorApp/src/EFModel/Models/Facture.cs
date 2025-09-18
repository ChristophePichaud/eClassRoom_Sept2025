using System;

public class Facture
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public DateTime Mois { get; set; }
    public decimal Montant { get; set; }
    public string Details { get; set; }
}