import sqlite3
import sys

try:
    conn = sqlite3.connect('GymPower.db')
    c = conn.cursor()

    c.execute("SELECT Id, Name FROM Products WHERE Name LIKE '%Creatine%' LIMIT 1")
    row = c.fetchone()
    if not row:
        print("Product not found")
        sys.exit(1)

    product_id = row[0]
    product_name = row[1]
    print(f"Found product {product_id}: {product_name}")

    c.execute("UPDATE Products SET ImageUrl = '/products/creatine_new_1.png' WHERE Id = ?", (product_id,))

    c.execute("DELETE FROM ProductImages WHERE ProductId = ?", (product_id,))

    variations = [
        (product_id, '/products/creatine_new_1.png'),
        (product_id, '/products/creatine_new_2.png'),
        (product_id, '/products/creatine_new_3.jpg')
    ]
    c.executemany("INSERT INTO ProductImages (ProductId, ImageUrl) VALUES (?, ?)", variations)

    conn.commit()
    print("Database updated successfully!")
    conn.close()

except Exception as e:
    print(f"Error: {e}")
