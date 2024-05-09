
--drop  table example.RawShapes

create table example.RawShapes
(
	Id int identity(1,1),
	ShapeName nvarchar(50) not null,
	Dimensions nvarchar(50) null,
	Units nvarchar(50) not null,
	Radius decimal(5,3),
	Area decimal(10,5)
)


insert into example.RawShapes
( ShapeName,Dimensions,Units,Radius, Area)

select 'Rectangle' , '5x5' , 'cm' , null , null  union all 
select 'Triangle' , '5x4x3' , 'cm' , null  , null    union all 
select 'Circle' , null , 'cm' , 4.4 , null 


select * from example.RawShapes