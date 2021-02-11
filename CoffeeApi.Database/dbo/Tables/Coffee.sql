CREATE TABLE [dbo].[Coffee] (
    [id]              INT           IDENTITY (1, 1) NOT NULL,
    [name]            VARCHAR (50)  NOT NULL,
    [description]     VARCHAR (150) NOT NULL,
    [caffeineContent] INT           NOT NULL,
    CONSTRAINT [PK_Coffee] PRIMARY KEY CLUSTERED ([id] ASC)
);

