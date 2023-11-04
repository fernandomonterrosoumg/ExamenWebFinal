-- Creación de la tabla 'genero'
CREATE TABLE genero (
  idgenero INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  descripcion VARCHAR2(45)
);

-- Creación de la tabla 'grupo'
CREATE TABLE grupo (
  idgrupo INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  nombre VARCHAR2(45),
  formacion DATE,
  disgregacion DATE
);

-- Creación de la tabla 'musico'
CREATE TABLE musico (
  idmusico INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  nombre VARCHAR2(45),
  instrumento VARCHAR2(45),
  lugarnacimiento VARCHAR2(45),
  fechanacimiento DATE,
  fechamuerte DATE
);

-- Creación de la tabla 'generosgrupos' (Tabla de relación muchos a muchos entre 'genero' y 'grupo')
CREATE TABLE generosgrupos (
  idgrupo INT,
  idgenero INT,
  CONSTRAINT pk_generosgrupos PRIMARY KEY (idgrupo, idgenero),
  CONSTRAINT fk_generosgrupos_grupo FOREIGN KEY (idgrupo) REFERENCES grupo (idgrupo),
  CONSTRAINT fk_generosgrupos_genero FOREIGN KEY (idgenero) REFERENCES genero (idgenero)
);

-- Creación de la tabla 'musicosgrupos' (Tabla de relación muchos a muchos entre 'musico' y 'grupo')
CREATE TABLE musicosgrupos (
  idgrupo INT,
  idmusico INT,
  instrumento VARCHAR2(45),
  fechainicio DATE,
  fechafin DATE,
  CONSTRAINT pk_musicosgrupos PRIMARY KEY (idgrupo, idmusico),
  CONSTRAINT fk_musicosgrupos_grupo FOREIGN KEY (idgrupo) REFERENCES grupo (idgrupo),
  CONSTRAINT fk_musicosgrupos_musico FOREIGN KEY (idmusico) REFERENCES musico (idmusico)
);