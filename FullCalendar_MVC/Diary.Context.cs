﻿

//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace FullCalendar_MVC
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class DiaryContainer : DbContext
{
    public DiaryContainer()
        : base("name=DiaryContainer")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<ContactInfo> ContactInfo { get; set; }

    public DbSet<Users> Users { get; set; }

    public DbSet<webpages_Membership> webpages_Membership { get; set; }

    public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }

    public DbSet<webpages_Roles> webpages_Roles { get; set; }

    public DbSet<AppointmentDiary> AppointmentDiary { get; set; }

    public DbSet<ArtWorks> ArtWorks { get; set; }

    public DbSet<Folder> Folder { get; set; }

    public DbSet<Salaat> Salaat { get; set; }

    public DbSet<SalaatTypes> SalaatTypes { get; set; }

    public DbSet<Slides> Slides { get; set; }

    public DbSet<IcomMembers> IcomMembers { get; set; }

    public DbSet<FamilyMembers> FamilyMembers { get; set; }

    public DbSet<countries> countries { get; set; }

    public DbSet<states> states { get; set; }

}

}

