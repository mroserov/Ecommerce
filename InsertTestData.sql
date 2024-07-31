USE [CatalogDb]
GO
-- Amount data
DECLARE @AmountData INT = 50;
-- Insertar las categorías
INSERT INTO dbo.Categories (Name)
VALUES
('Nike'),
('Adidas'),
('New Balance'),
('Kids'),
('Sportwear'),
('Soccer'),
('Sandals'),
('Outdoor');

-- Crear una tabla temporal para las imágenes y sus categorías correspondientes
CREATE TABLE #TempProductImages (
    ImageUrl NVARCHAR(MAX),
    CategoryList NVARCHAR(MAX) -- Almacena una lista de categorías separadas por comas
);

-- Insertar las imágenes con sus categorías correspondientes
INSERT INTO #TempProductImages (ImageUrl, CategoryList)
VALUES
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/9bc016bc-cd7a-49cc-a399-47930b00c59f/tenis-dunk-low-retro-5FQWGR.png', 'Nike,Sportwear'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/24d898c4-0400-457e-be38-fd9d9b36e4e2/tenis-zoom-vomero-5-MgsTqZ.png', 'Nike,Sportwear'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/3e8455ad-c00c-4996-a85a-b5c4d38c6ae2/tenis-v2k-run-ZKMJLX.png', 'Nike,Sportwear'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/2fad5de7-d114-4595-a4c6-4d96fde6ae0d/tenis-air-max-2017-BVqnkV.png', 'Nike,Sportwear'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/u_126ab356-44d8-4a06-89b4-fcdcc8df0245,c_scale,fl_relative,w_1.0,h_1.0,fl_layer_apply/89bd904a-dfae-4f50-930e-768e016750e0/tenis-jumpman-mvp-gzmjDz.png', 'Nike,Sportwear'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/eede2fe02aeb43e7b5daaf5f00f64aef_9366/guayos-deportivo-ii-multiterreno.jpg', 'Adidas,Outdoor'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/fd41aca7e5044c11b94877600f1d5de8_9366/tenis-de-trail-running-terrex-soulstride-flow.jpg', 'Adidas,Outdoor'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/8d547c9c54a64d6dadb299183bad2e34_9366/tenis-trailmaker-2.jpg', 'Adidas,Outdoor'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/78dc08c16bd84cd3bef2ebeffc70a30c_9366/tenis-disney-monofit-para-bebe.jpg', 'Adidas,Kids,Sportwear'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/d5b2a49bb3e44114ba3dcea57d389d0f_9366/guayos-predator-24-league-terreno-firme-sin-cordones.jpg', 'Adidas,Kids,Sportwear'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/16f8308f85cb4616afb6cfd462ed8f76_9366/tenis-monofit-x-disney-ninos.jpg', 'Adidas,Kids,Sportwear'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/d182cba78a73423ebcc087c7705f5021_9366/guayos-copa-pure-ii-league-pasto-sintetico.jpg', 'Adidas,Kids,Sportwear'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/57c1ea20202140c28473e4eedf53248f_9366/tenis-star-wars-grand-court-2.0-kids.jpg', 'Kids,Sportwear'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/ad80e112-62cf-4b1b-8ae8-0285a69ebf21/tacos-de-f%C3%BAtbol-mg-y-grandes-jr-phantom-luna-2-academy-nLpHdL.png', 'Kids,Soccer'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/79b09474-13a2-42c8-b791-3e693f96782b/tacos-de-f%C3%BAtbol-de-corte-low-para-terrenos-m%C3%BAltiples-y-grandes-jr-tiempo-legend-10-academy-3tpbSH.png', 'Kids,Soccer'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/cd691fa7-0f6c-4c22-a011-c6dcf875874f/tacos-de-f%C3%BAtbol-de-corte-high-ic-talla-grande-jr-superfly-9-club-mercurial-dream-speed-SVMHxB.png', 'Kids,Soccer'),
('https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/580e052a538c41cfaf4aac6f01260bee_9366/Chanclas_adilette_Aqua_Azul_FY8071_09_standard.jpg', 'Adidas,Sandals'),
('https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/9b55ab038cd64554a0b8bed78b61aede_9366/Sandalias_Adilette_Comfort_x_Marvel_Kids_Blanco_ID8029_011_hover_standard.jpg', 'Adidas,Sandals'),
('https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/d6d4dde556d744f28e5a4711a6bfb8a9_9366/Sandalias_de_Natacion_Altaventure_Sport_Negro_ID7859_04_standard.jpg', 'Adidas,Sandals'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/b638f655103f47eba01c2ea67cee0851_9366/tenis-adidas-vl-court-3.0.jpg', 'Adidas,Sportwear'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/5cb689c986e04d22b83292c38022f6f9_9366/tenis-adidas-grand-court-cloudfoam-lifestyle-court-comfort.jpg', 'Adidas,Sportwear'),
('https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/219e45b725b947f0986e5ca5b6f3cf50_9366/guayos-copa-pure-2-league-cesped-natural-seco.jpg', 'Adidas,Soccer'),
('https://static.nike.com/a/images/q_auto:eco/t_product_v1/f_auto/dpr_1.3/h_466,c_limit/57a3a879-3d85-4d04-9c3a-0d5d9ccc7972/tacos-de-f%C3%BAtbol-low-para-terreno-firme-mercurial-vapor-16-elite-electric-d1TXqP.png', 'Nike,Soccer'),
('https://static.nike.com/a/images/q_auto:eco/t_product_v1/f_auto/dpr_1.3/h_466,c_limit/07fdb3b0-feee-40f9-ba24-f299c863aa56/tacos-de-f%C3%BAtbol-para-pasto-artificial-tiempo-legend-10-elite-4ptLMt.png', 'Nike,Soccer'),
('https://static.nike.com/a/images/q_auto:eco/t_product_v1/f_auto/dpr_1.3/h_466,c_limit/bf330822-4564-4f8b-aed8-caf39e71961a/tacos-de-f%C3%BAtbol-low-ag-pro-mercurial-vapor-16-elite-4dhLMM.png', 'Nike,Soccer'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/641c2ad1-800d-469a-9b5b-55b69ec6be60/tacos-de-f%C3%BAtbol-fg-de-corte-high-phantom-luna-2-elite-mCmzLD.png', 'Nike,Soccer'),
('https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/bf94217a-78bf-44c6-b5a4-558ae198fffc/tacos-de-f%C3%BAtbol-high-mg-phantom-luna-2-academy-Nht4ZV.png', 'Nike,Soccer'),
('https://nb.scene7.com/is/image/NB/u9060gry_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear'),
('https://nb.scene7.com/is/image/NB/ufpwrk1_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear'),
('https://nb.scene7.com/is/image/NB/ws327bl_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear'),
('https://nb.scene7.com/is/image/NB/bb550cpb_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear'),
('https://nb.scene7.com/is/image/NB/ms41fmbk_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear'),
('https://nb.scene7.com/is/image/NB/mtm10lk1_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear'),
('https://nb.scene7.com/is/image/NB/st1flh45_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear'),
('https://nb.scene7.com/is/image/NB/ms42tbk2_nb_05_i?$pdpflexf2$&qlt=80&fmt=webp&wid=440&hei=440', 'New Balance,Sportwear');

-- Crear una tabla temporal para nombres de productos
CREATE TABLE #TempProductNames (
    Name NVARCHAR(100),
    BaseCategoryId INT -- Una categoría base para el nombre del producto
);

-- Insertar nombres de productos aleatorios con sus respectivas categorías base
INSERT INTO #TempProductNames (Name, BaseCategoryId)
VALUES
('Nike Air Max', 1),
('Nike Air Force 1', 1),
('Nike React', 1),
('Nike Blazer', 1),
('Nike Dunk', 1),
('Nike Zoom', 1),
('Nike Vaporfly', 1),
('Adidas Ultraboost', 2),
('Adidas NMD', 2),
('Adidas Stan Smith', 2),
('Adidas Superstar', 2),
('Adidas Gazelle', 2),
('Adidas Samba', 2),
('Adidas Yeezy', 2),
('New Balance 990', 3),
('New Balance 1080', 3),
('New Balance 574', 3),
('New Balance 860', 3),
('New Balance Fresh Foam', 3);

-- Crear una tabla temporal para descripciones de productos
CREATE TABLE #TempProductDescriptions (
    Description NVARCHAR(500)
);

-- Insertar descripciones de productos aleatorias
INSERT INTO #TempProductDescriptions (Description)
VALUES
('Comfortable and stylish sneakers perfect for everyday wear.'),
('High-performance running shoes with superior cushioning.'),
('Classic design with a modern twist, ideal for casual outfits.'),
('Lightweight and breathable shoes for intense workouts.'),
('Durable and supportive shoes designed for all-day comfort.');

-- Crear una tabla temporal para descripciones de productos
CREATE TABLE #TempCategory (
    Name NVARCHAR(500)
);

-- Insertar 1000 productos en la tabla [Products]
DECLARE @Counter INT = 1;

WHILE @Counter <= @AmountData
BEGIN
    DECLARE @RandomProductName NVARCHAR(100);
    DECLARE @RandomBaseCategoryId INT;
    DECLARE @UniqueProductName NVARCHAR(100);
    DECLARE @RandomProductDescription NVARCHAR(500);
    DECLARE @RandomPrice DECIMAL(18,2);
    DECLARE @RandomStock INT;
    DECLARE @RandomDiscount DECIMAL(18,2);
    DECLARE @RandomImageUrl NVARCHAR(MAX);
    DECLARE @RandomCategoryList NVARCHAR(500);
    DECLARE @CreatedAt DATETIME2(7);
    DECLARE @UpdatedAt DATETIME2(7);
    DECLARE @InsertedProductId INT;

    -- Obtener un nombre de producto base y su categoría aleatorios
    SELECT TOP 1 @RandomProductName = Name, @RandomBaseCategoryId = BaseCategoryId
    FROM #TempProductNames
    ORDER BY NEWID();

    -- Obtener una descripción de producto aleatoria
    SELECT TOP 1 @RandomProductDescription = Description
    FROM #TempProductDescriptions
    ORDER BY NEWID();

    -- Generar otros valores aleatorios
    SET @RandomPrice = CAST((RAND() * (200 - 50) + 50) AS DECIMAL(18,2)); -- Precio entre 50 y 200
    SET @RandomStock = CAST((RAND() * (100 - 0) + 0) AS INT); -- Stock entre 10 y 100
    SET @RandomDiscount = CAST((RAND() * (20 - 0) + 0) AS DECIMAL(18)); -- Descuento entre 0 y 20

    -- Obtener una imagen y su lista de categorías aleatorias
    SELECT TOP 1 @RandomImageUrl = ImageUrl, @RandomCategoryList = CategoryList
    FROM #TempProductImages
    ORDER BY NEWID();

    SET @CreatedAt = GETDATE();
    SET @UpdatedAt = GETDATE();

    -- Generar un nombre de producto único concatenando el contador
    SET @UniqueProductName = CONCAT(@RandomProductName, ' ', @Counter, ' ', @RandomPrice);

    -- Insertar el producto en la tabla [Products]
    INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[Price]
           ,[Stock]
           ,[Discount]
           ,[Slug]
           ,[ImageUrl]
           ,[CreatedAt]
           ,[UpdatedAt])
     VALUES
           (@UniqueProductName
           ,@RandomProductDescription
           ,@RandomPrice
           ,@RandomStock
           ,@RandomDiscount
           ,LOWER(REPLACE(@UniqueProductName, ' ', '-')) -- Slug
           ,@RandomImageUrl
           ,@CreatedAt
           ,@UpdatedAt);

    -- Obtener el ID del producto insertado
    SET @InsertedProductId = SCOPE_IDENTITY();
	DELETE FROM #TempCategory;

	INSERT INTO #TempCategory (Name)
	SELECT value FROM STRING_SPLIT(@RandomCategoryList, ',');

    -- Insertar las relaciones en la tabla [ProductCategory] para cada categoría
    DECLARE @CategoryId INT;
	DECLARE @CategoryListCursor CURSOR;
	SET @CategoryListCursor = CURSOR FOR
		SELECT Id FROM dbo.Categories WHERE Name IN(SELECT value FROM STRING_SPLIT('Nike,Adidas', ','));
        --SELECT Name AS 'Name' FROM #TempCategory;

    OPEN @CategoryListCursor;
    FETCH NEXT FROM @CategoryListCursor INTO @CategoryId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
		SELECT Id FROM dbo.Categories WHERE Id = @CategoryId
		IF @CategoryId IS NOT NULL
		BEGIN
			INSERT INTO dbo.ProductCategory (CategoryId, ProductId)
			VALUES (@CategoryId, @InsertedProductId);
		END
		
        FETCH NEXT FROM @CategoryListCursor INTO @CategoryId;
    END

    CLOSE @CategoryListCursor;
    DEALLOCATE @CategoryListCursor;

    SET @Counter = @Counter + 1;
END

-- Limpiar las tablas temporales
DROP TABLE #TempCategory;
DROP TABLE #TempProductNames;
DROP TABLE #TempProductDescriptions;
DROP TABLE #TempProductImages;
