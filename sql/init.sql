drop database if exists railroad_map;
create database railroad_map DEFAULT CHARACTER SET utf8 DEFAULT COLLATE utf8_polish_ci;
drop user if exists RailroadUser@'%';
create user RailroadUser@'%' identified by 'ZXIT1HaYDwdz69A9m7yhiIq8';
grant all privileges on railroad_map.* to RailroadUser@'%';
flush privileges;
use railroad_map;
create table category(
	idcategory int primary key auto_increment,
    discriminant varchar(4) not null unique,
    fullname text not null);
create table railroad_lines(
	linenr int primary key,
    linename text not null);
create table railroad_points(
	postid int primary key auto_increment,
    postname varchar(256) not null,
    platform bool not null,
    requeststop bool not null,
    loadingpoint bool not null,
    idcategory int not null,
    constraint FK_RP_C foreign key (idcategory) references category(idcategory));
create table points_in_line(
	linenr int,
    postid int,
    kilometer double not null,
    constraint PK_PIL primary key (linenr, postid),
    constraint FK_PIL_RL foreign key (linenr) references railroad_lines(linenr),
    constraint FK_PIL_RP foreign key (postid) references railroad_points(postid));
use railroad_map;