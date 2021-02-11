CREATE TABLE [dbo].[Rating] (
    [id]       INT           IDENTITY (1, 1) NOT NULL,
    [coffeeId] INT           NOT NULL,
    [rating]   INT           NOT NULL,
    [comment]  VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Rating] FOREIGN KEY ([coffeeId]) REFERENCES [dbo].[Coffee] ([id])
);

