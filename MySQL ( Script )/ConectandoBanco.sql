create database if not exists escola;
use escola;

create table if not exists alunos (
  Id int auto_increment primary key,
  Nome VARCHAR(100) NOT NULL,
  Idade INT NOT NULL,
  Curso VARCHAR(50) NOT NULL
);

select * from alunos;
delete from alunos;
truncate table alunos;
