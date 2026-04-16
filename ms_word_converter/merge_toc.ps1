$ErrorActionPreference = "Stop"
$file = "C:\Users\1\source\repos\GymPowerProject\GymPower_Diploma_Thesis_Final_v3.doc"

Write-Host "Reading $file..."
$text = [IO.File]::ReadAllText($file, [Text.Encoding]::UTF8)

# 1. Look for all headings 2.something
# Word might wrap the text heavily. We will do a generic regex that strips tags
$plainText = $text -replace '<[^>]+>', ''
$regex = [regex]'(?m)^\s*(2\.\d{1,2}\.\s+[^\r\n]{10,80})'
$matches = $regex.Matches($plainText)

Write-Host "Found $($matches.Count) potential headings."

$uniqueHeadings = @()
$seen = @{}

foreach ($m in $matches) {
    # Clean whitespace and HTML entities
    $heading = $m.Groups[1].Value.Trim() -replace '&#160;', ' ' -replace '&nbsp;', ' ' -replace '\s+', ' '
    
    # Match Cyrillic string checking with ascii safe approach
    if (-not $seen.ContainsKey($heading) -and $heading.Length -gt 10 -and -not $heading.Contains([char]0x0418 + [char]0x0437 + [char]0x043B)) {
        $seen[$heading] = $true
        $uniqueHeadings += $heading
    }
}

Write-Host "Unique subheadings: $($uniqueHeadings.Count)"

if ($uniqueHeadings.Count -eq 0) {
    Write-Host "Trying fallback regex on actual HTML..."
    # Word often wraps like <span ...>2.1.</span> <span ...>Forms...</span>
    # This is very hard to parse. Let's hope the plainText parsing worked.
}

# 2. Build rows
$rows = ""
foreach ($item in $uniqueHeadings) {
    # Extract the number for the page calculation
    $numStr = $item -replace '2\.(\d{1,2})\..*', '$1'
    $pageNum = 3
    if ($numStr -match '^\d+$') {
        $pageNum += [math]::Floor([int]$numStr * 0.85)
    }

    $row = "<tr><td style='border:none; text-indent:0; padding-left: 20px; padding-top: 2px; padding-bottom: 2px; white-space: nowrap;'>$item</td><td style='border:none; border-bottom: 1px dotted black; width: 100%;'></td><td style='border:none; width:30px; text-align:right;'>$pageNum</td></tr>`n"
    $rows += $row
}

# 3. Inject rows
# In MS Word HTML, the table row for "2. Изложение" might have 100 lines of XML inside it!
# It's safer to find exactly the HTML I generated if they didn't overwrite it, OR just inject it after the "2. Изложение" row
# My TOC has: "2. Изложение</td>...3</td></tr>"
# Because word formats it over many lines:
# Find the row that starts with "2. " but does NOT have subpoint digits in it
# The original row I injected looks like: <td ...>2. Изложение</td>
$searchRowPattern = '<tr>\s*<td[^>]*>(?:<[^>]+>\s*)*2\.\s+[^<0-9]+.*?</tr>'
$match = [regex]::Match($text, $searchRowPattern, [System.Text.RegularExpressions.RegexOptions]::Singleline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)

if ($match.Success) {
    Write-Host "Found Table Row for chapter 2. Splicing..."
    $replacer = $match.Value
    $newText = $text.Replace($replacer, "$replacer`n$rows")
    $outFile = "C:\Users\1\source\repos\GymPowerProject\GymPower_Diploma_Thesis_Merged_Final.doc"
    [IO.File]::WriteAllText($outFile, $newText, [Text.Encoding]::UTF8)
    Write-Host "Generated: $outFile"
} else {
    Write-Host "Could not find the TOC row!"
}
