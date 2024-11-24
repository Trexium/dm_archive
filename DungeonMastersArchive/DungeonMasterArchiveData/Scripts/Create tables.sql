

create table [User]
(
	Id int identity(1,1) primary key,
	UserName nvarchar(255) not null

)

create table AccessType
(
	Id int identity(1, 1) primary key,
	DisplayText nvarchar(255)
)


create table ArticleUserAccess
(
	Id int identity(1, 1) primary key,
	CreatedAt datetime default getdate(),
	UserId int foreign key references [User](Id),
	AccessTypeId int foreign key references AccessType(Id)
)


create table Campaign
(
	Id int identity(1, 1) primary key, 
	CreatedAt datetime default getdate(),
	CreatedBy int foreign key references [User](Id),
	UpdatedAt datetime null,
	UpdateBy int foreign key references [User](Id),
	CampaignName nvarchar(255) not null
)

create table ArticleType
(
	Id int identity(1, 1) primary key,
	DisplayText nvarchar(255) not null
)



create table Article
(
	Id int identity(1, 1) primary key,
	CreatedAt datetime default getdate(),
	CreatedBy int not null foreign key references [User](Id),
	UpdatedAt datetime null,
	UpdateBy int foreign key references [User](Id),
	IsDeleted bit default 0,
	IsPublished bit default 0,
	ArticleName nvarchar(255) not null,
	CampaignId int not null foreign key references Campaign(Id),
	ArticleTypeId int not null foreign key references ArticleType(Id),
	ArticleText nvarchar(max),
	ArticleYear int null,
	ArticleMonth int null,
	ArticleDay int null
)

create table ArticleLink
(
	Id int identity(1, 1) primary key,
	ParentArticleId int not null foreign key references Article(Id),
	ChildArticleId int not null foreign key references Article(Id)
)

create table ArticleTag
(
	Id int identity(1, 1) primary key,
	ArticleId int not null foreign key references Article(Id),
	Tag nvarchar(255) not null
)

create table ArticleImage
(
	Id int identity(1, 1) primary key,
	CreatedAt datetime default getdate(),
	ArticleId int not null foreign key references Article(Id),
	CampaignId int not null foreign key references Campaign(Id),
	Title nvarchar(255),
	ImageUrl nvarchar(512)
)


create table GenericValueStore
(
	[Group] nvarchar(255) not null,
	[Key] nvarchar(25) not null,
	[Value] nvarchar(max) not null,
	constraint PK_Group_Key primary key clustered ([Group], [Key])
)




