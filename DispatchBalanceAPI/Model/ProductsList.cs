
﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DispatchBalanceAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(Internal_codes))]
public class ProductsList
{
    public string Internal_codes { get; set; }
    public string Bar_code { get; set; }
    public string Large_name { get; set; }
    public string Short_name { get; set; }
    public string Organization { get; set; }
    public string Brand_fp_u_code { get; set; }
    public string Brand_fp { get; set; }
    public string Categoria_u_code { get; set; }
    public string Categoria { get; set; }
    public string Linea_u_code { get; set; }
    public string Linea { get; set; }
    public string Subline_dsd_u_code { get; set; }
    public string Subline_dsd { get; set; }
    public string Sales_unit { get; set; }
    public float Net_content { get; set; }
    public int Packaging_capacity { get; set; }
    public string Type_of_presentation_fp { get; set; }
    public int Units_per_package { get; set; }
    public string Status { get; set; }
    public string Codigo_envase { get; set; }
    public string? Codigo_ADI { get; set; }
    public string Clave_sat { get; set; }
    public string Unit_key { get; set; }
    public int Shelf_life { get; set; }
    public int Return_limit { get; set; }
    public string Channel { get; set; }
    public string Management { get; set; }
    public string Region { get; set; }
    public string Zone { get; set; }
    public string Whol_price { get; set; }
    public string Iva { get; set; }
    public string Ieps { get; set; }
    public int Container_capacity { get; set; }
    public int Beds_per_container { get; set; }
    public string? Type_accommodation { get; set; }
    public string Type_qp { get; set; }
    public int Clave_margen_dias { get; set; }
    public string Ultima_clave_recibida { get; set; }
    public string Manejo_por_semana { get; set; }



    public class ProductListFinalGroup
    {
        public string Internal_codes { get; set; }
        public string Large_name { get; set; }        
        public int Entrada { get; set; }
        public int Salida { get; set; }
    }

    public class ProductListFirstGroup
    {
        public string Internal_codes { get; set; }
        public string Large_name { get; set; }
        public string Quantity { get; set; }
        public string CeveCode { get; set; }
        public string SaleDate { get; set; }
        public string OutgoingIncome { get; set; }
        public string TransactionCode { get; set; }
        public int Entrada { get; set; }
        public int Salida { get; set; }
    }
}
