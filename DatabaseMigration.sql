IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Clients] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [ContactDetails] nvarchar(max) NOT NULL,
    [Region] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
);

CREATE TABLE [Contracts] (
    [Id] int NOT NULL IDENTITY,
    [ClientId] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [ServiceLevel] nvarchar(max) NOT NULL,
    [SignedAgreementPath] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Contracts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Contracts_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ServiceRequests] (
    [Id] int NOT NULL IDENTITY,
    [ContractId] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CostUSD] decimal(18,2) NOT NULL,
    [CostZAR] decimal(18,2) NOT NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_ServiceRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ServiceRequests_Contracts_ContractId] FOREIGN KEY ([ContractId]) REFERENCES [Contracts] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Contracts_ClientId] ON [Contracts] ([ClientId]);

CREATE INDEX [IX_ServiceRequests_ContractId] ON [ServiceRequests] ([ContractId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260516131912_InitialCreate', N'10.0.8');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Contracts]') AND [c].[name] = N'SignedAgreementPath');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Contracts] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Contracts] ALTER COLUMN [SignedAgreementPath] nvarchar(max) NULL;

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Contracts]') AND [c].[name] = N'ServiceLevel');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Contracts] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [Contracts] ALTER COLUMN [ServiceLevel] nvarchar(max) NULL;

DECLARE @var2 nvarchar(max);
SELECT @var2 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'Region');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var2 + ';');
ALTER TABLE [Clients] ALTER COLUMN [Region] nvarchar(max) NULL;

DECLARE @var3 nvarchar(max);
SELECT @var3 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Clients]') AND [c].[name] = N'ContactDetails');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Clients] DROP CONSTRAINT ' + @var3 + ';');
ALTER TABLE [Clients] ALTER COLUMN [ContactDetails] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260517105147_AddContracts', N'10.0.8');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Contracts] ADD [FilePath] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260517134246_AddContractFilePath', N'10.0.8');

COMMIT;
GO

