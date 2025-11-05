create database if not exists escola;
use escola;

create table if not exists alunos (
  Id int auto_increment primary key,
  Nome varchar(100) not null,
  Idade int not null,
  Curso varchar(50) not null
);

# Testes
select * from alunos;
delete from alunos;
truncate table alunos;
