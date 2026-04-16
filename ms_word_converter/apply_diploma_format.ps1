$ErrorActionPreference = "Stop"
# Read v3.doc which is basically MHTML now
$inFile = "C:\Users\1\source\repos\GymPowerProject\GymPower_Diploma_Thesis_Final_v3.doc"
$outFile = "C:\Users\1\source\repos\GymPowerProject\Martin_Angelov_12.doc"

Write-Host "Reading file..."
$text = [IO.File]::ReadAllText($inFile, [Text.Encoding]::GetEncoding("windows-1251"))

# 1. Update Author Property
$text = $text -replace '<o:Author>.*?</o:Author>', '<o:Author>Martin Angelov</o:Author>'

# 2. Add Native TOC
$nativeTOC = @"
<br clear='all' style='page-break-before:always' />
<h2 style='text-align:center;'>СЪДЪРЖАНИЕ</h2>
<br>
<p class=MsoToc1>
<!--[if supportFields]><span style='mso-element:field-begin'></span><span style='mso-spacerun:yes'> </span>TOC \o "1-3" \h \z \u <span style='mso-element:field-separator'></span><![endif]-->
<span style='mso-no-proof:yes'>[Съдържанието ще се генерира тук. В Word натиснете Десен Бутон върху този текст -> 'Update Field' / 'Обнови полето'!]</span>
<!--[if supportFields]><span style='mso-element:field-end'></span><![endif]-->
</p>
<br clear='all' style='page-break-before:always' />
"@

# The old TOC has this distinct beginning/end because it was wrapped in a table
# Note: we need to find the old manual TOC and remove it. 
# It starts around СЪДЪРЖАНИЕ and goes until right before 1. Увод
$startIdx = $text.IndexOf("СЪДЪРЖАНИЕ")
if ($startIdx -gt 0) {
    # Find the real start of the "1. Увод" heading body
    # It might look like: >1. Увод< or 1. Увод<o:p>
    $endRegex = [regex]'(<h2[^>]*>(<span[^>]*>)?1\.\s*Увод)'
    $match = $endRegex.Match($text, $startIdx + 10)
    if ($match.Success) {
         # We want to replace everything from the first <h2>СЪДЪРЖАНИЕ</h2> up to the match
         $beforeToc = $text.Substring(0, $startIdx - 50) # Go a bit before to hit the tag start
         # Seek back to find the actual start of the TOC heading tag
         $h2Start = $text.LastIndexOf("<h2", $startIdx)
         if ($h2Start -gt 0) {
             $beforeToc = $text.Substring(0, $h2Start)
         }
         
         $afterToc = $text.Substring($match.Index)
         $text = $beforeToc + $nativeTOC + $afterToc
         Write-Host "Replaced manual TOC with Native MS Word TOC Field."
    }
}

# 3. Strip red text instructions.
# In the original, it was [ВЪВЕДЕТЕ ГРАД] etc. But the user said "След попълването... всички текстове в червено да се премахнат".
# They also said "аз ще се оправя с тези данни". Meaning they probably ALREADY filled them in manually in 35Pages.doc, but I am operating on v3.doc where they might still be blank.
# Let's cleanly replace the placeholders with standard blank underscores or just remove the brackets so they can do it.
$text = $text -replace '\[ДА СЕ ПОПЪЛНИ\]', '________________'
$text = $text -replace '\[ВЪВЕДЕТЕ ГРАД\]', '________________'
$text = $text -replace '\[ПЪРВО ИМЕ\] \[ПРЕЗИМЕ\] \[ФАМИЛИЯ\]', 'Мартин Ангелов'
$text = $text -replace '\[ВЪВЕДЕТЕ ПРОФЕСИЯ\]', '________________'
$text = $text -replace '\[ВЪВЕДЕТЕ СПЕЦИАЛНОСТ\]', '________________'
$text = $text -replace '\[ВЪВЕДЕТЕ КЛАС\]', '12'
$text = $text -replace '\[ИМЕ И ФАМИЛИЯ\]', '________________'

# 4. Enforce STRICT Diploma Page Limits using pure CSS
# 1900 chars ~ 30 lines. Line-height 23pt forces exactly 30 lines on A4 with 2.5cm margins.
$customStyle = @"
<style>
/* Стил ""Дипломна работа"" според изискванията */
@page WordSection1 {
    size: 595.3pt 841.9pt;
    margin: 2.5cm 2.5cm 2.5cm 2.5cm; /* Strict square margins */
    mso-header-margin: 35.4pt;
    mso-footer-margin: 35.4pt;
    mso-paper-source: 0;
}
div.WordSection1 { page: WordSection1; }
p, li, div {
    mso-style-name: "Дипломна работа";
    font-family: "Times New Roman", serif !important;
    font-size: 12.0pt !important;
    line-height: 23pt !important; /* 30 lines exactly per page! */
    text-align: justify !important;
}
h1, h2, h3 {
    font-family: "Times New Roman", serif !important;
    font-weight: bold !important;
}
</style>
"@

$text = $text -replace '</head>', "$customStyle`n</head>"

# Save
[IO.File]::WriteAllText($outFile, $text, [Text.Encoding]::GetEncoding("windows-1251"))
Write-Host "SUCCESS: Written $outFile"
