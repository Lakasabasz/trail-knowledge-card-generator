import pandas as pd


def load(file: str):
    with open(file, mode='r', encoding="UTF-8") as source:
        rawdata = source.read().split("\n")
        print(len(rawdata))
        headers = rawdata[2].split(";")
        parsed = []
        for line in rawdata[4:-1]:
            values = line.split(";")
            parsed.append(dict(zip(headers, values)))
        return parsed


def find_value(row, dataframe: pd.DataFrame):
    return dataframe.loc[
              (dataframe["Nazwa punktu"] == row["Nazwa punktu"]) &
              (dataframe["Wyróżnik"] == row["Wyróżnik"]) &
              (dataframe["Peron"] == row["Peron"])
              ]["postid"].values[0]


if __name__ == "__main__":
    loaded = load("../data/pkpplk_raw.csv")
    df = pd.DataFrame(loaded)
    dictionary = df.to_dict(orient="list")
    print(df.columns)

    # Categories
    categories = pd.DataFrame(df["Wyróżnik"].drop_duplicates().copy())
    categories["fullname"] = ""
    categories.reset_index(drop=True, inplace=True)
    categories.index.name = "idcategory"
    categories.index += 1
    categories.reset_index(inplace=True)

    # Railroad lines
    lines = df[["Nr linii", "Nazwa linii"]].drop_duplicates().rename({"Nr linii": "linenr", "Nazwa linii": "linename"}, axis=1)

    # Railroad points
    points = df[["Nazwa punktu", "Peron", "Przystanek na żądanie", "Punkt ładunkowy", "Wyróżnik"]].drop_duplicates() \
        .reset_index(drop=True)
    points.index += 1
    points.index.name = "postid"
    points["idcategory"] = points["Wyróżnik"].map(categories.set_index("Wyróżnik")["idcategory"])
    points.reset_index(inplace=True)
    old = pd.get_option("display.max_columns")
    pd.set_option("display.max_columns", None)
    pd.set_option("display.max_columns", old)

    # Points in line
    pil = df[["Nr linii", "Nazwa punktu", "Wyróżnik", "Peron", "Km osi"]]
    pil["postid"] = pil.apply(lambda x: find_value(x, points), axis=1)
    pil["Km osi"] = pil.apply(lambda x: float(str(x["Km osi"]).replace(",", ".")), axis=1)
    pil = pil.drop(["Nazwa punktu", "Wyróżnik", "Peron"], axis=1)
    pil.rename({"Nr linii": "linenr", "Km osi": "kilometer"}, inplace=True, axis=1)

    categories.rename({"Wyróżnik": "discriminant"}, inplace=True, axis=1)
    points.rename({"Nazwa punktu": "postname", "Peron": "platform", "Przystanek na żądanie": "requeststop",
                   "Punkt ładunkowy": "loadingpoint"}, inplace=True, axis=1)
    points[["platform", "requeststop", "loadingpoint"]] = points[["platform", "requeststop", "loadingpoint"]].applymap(lambda x: int(not not x))
    points.drop(["Wyróżnik"], axis=1, inplace=True)
    categories.to_csv("../data/categories.csv", index=False, encoding="windows-1250")
    lines.to_csv("../data/lines.csv", index=False, encoding="windows-1250")
    points.to_csv("../data/posts.csv", index=False, encoding="windows-1250")
    pil.to_csv("../data/pil.csv", index=False, encoding="windows-1250")
