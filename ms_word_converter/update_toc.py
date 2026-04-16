import re

filename = r'C:\Users\1\source\repos\GymPowerProject\GymPower_Diploma_Thesis_Final_v3.doc'

content = ""
for enc in ['utf-8', 'windows-1251', 'cp1251', 'utf-16']:
    try:
        with open(filename, 'r', encoding=enc) as f:
            content = f.read()
        print(f"Read with encoding {enc}")
        break
    except Exception:
        pass

if not content:
    print("Could not read file.")
    exit(1)

# Extract raw text from MHTML / HTML
raw_text = re.sub(r'<[^>]+>', ' ', content)
raw_text = raw_text.replace('&nbsp;', ' ').replace('&#160;', ' ')
raw_text = re.sub(r'\s+', ' ', raw_text)

# Find all instances of " 2.X. Something " 
matches = re.findall(r'(2\.\d{1,2}\.\s+[A-Za-zА-Яа-я0-9\s()&;,.-]+?)(?=\s*2\.\d|\s*<|\s*3\.)', raw_text)

seen = set()
clean_matches = []
for m in matches:
    m = m.strip()
    # Limit length so we don't capture the entire chapter paragraph
    if len(m) > 120:
        # just cut it off at a reasonable sentence end or length
        m = m[:m.find(' ', 80)] if m.find(' ', 80) != -1 else m[:80]
        m = m.strip()
    
    if m not in seen:
        seen.add(m)
        clean_matches.append(m)

# Now we construct the dynamic HTML rows
toc_rows = []
for m in clean_matches:
    try:
        num_str = m.split('.')[1]
        num = int(num_str)
        estimated_page = 3 + int(num * 0.85)
        toc_rows.append(f"<tr><td style='border:none; text-indent:0; padding-left: 20px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;'>{m}</td><td style='border:none; border-bottom: 1px dotted black; width: 100%;'></td><td style='border:none; width:30px; text-align:right;'>{estimated_page}</td></tr>")
    except:
        pass

if not toc_rows:
    print("No TOC rows built.")
    exit(1)

print("Found and Built Rows:")
print(str(len(toc_rows)))

# Now we need to REPLACE the TOC table in the DOC HTML
# In my previous code, I injected <tr>...2. Изложение...</tr>
# Let's find that line in the HTML content
search_str = r"<tr><td style='border:none; text-indent:0; padding:2px; white-space: nowrap;'>2. Изложение</td><td style='border:none; border-bottom: 1px dotted black; width: 100%;'></td><td style='border:none; width:30px; text-align:right;'>3</td></tr>"

replacement = search_str + "\n" + "\n".join(toc_rows)

if search_str in content:
    content = content.replace(search_str, replacement)
    with open(r'C:\Users\1\source\repos\GymPowerProject\GymPower_Diploma_Thesis_Merged.doc', 'w', encoding='utf-8') as f:
        f.write(content)
    print("SUCCESS: Merged.doc created.")
else:
    print("Could not find the exact old TOC row to replace. Maybe the encoding or spacing is different.")
    # Fallback search
    if "2. Изложение" in content:
        print("We found '2. Изложение' but the exact row didn't match.")

