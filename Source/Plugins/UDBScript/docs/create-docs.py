import xmltodict
import glob
import pprint
import re

pp = pprint.PrettyPrinter(indent=4)

def determine_text_type(text):
    signature = text.replace('public ', '').replace ('static ', '').replace('Wrapper', '')
    if 'class ' in text or 'struct ' in text:
        return 'global', None
    if '(' not in text:
        return 'properties', re.sub(r'[^\s]+\s+', r'', signature).rstrip(';')
    signaturefields = signature.split('(')
    signature = re.sub(r'[^\s]+\s+', r'', signaturefields[0]) + '(' + re.sub(r'([^\s]+) ([^,]+)(,?\s*)', r'\2\3', signaturefields[1])
    fields = text.split()
    if fields[0] == 'public' and ('Wrapper(' in fields[1] or 'QueryOptions(' in fields[1]):
        return 'constructors', signature
    elif fields[1] == 'static':
        return 'staticmethods', signature
    return 'methods', signature

def get_sorted_comment_texts(texts):
    text = ''
    for  t in sorted(texts.keys()):
        text += texts[t]
    return text

topics = {
    'GameConfiguration': [ '../API/GameConfigurationWrapper.cs' ],
    'Angle2D': [ '../API/Angle2DWrapper.cs' ],
    'Data': [ '../API/DataWrapper.cs' ],
    'ImageInfo': [ '../API/ImageInfo.cs' ],
    'Line2D': [ '../API/Line2DWrapper.cs' ],
    'Linedef': [ '../API/LinedefWrapper.cs', '../API/MapElementWrapper.cs' ],
    'Map': [ '../API/MapWrapper.cs' ],
    'Sector': [ '../API/SectorWrapper.cs', '../API/MapElementWrapper.cs' ],
    'Sidedef': [ '../API/SidedefWrapper.cs', '../API/MapElementWrapper.cs' ],
    'Thing': [ '../API/ThingWrapper.cs', '../API/MapElementWrapper.cs' ],
    'UDB': [ '../API/UDBWrapper.cs' ],
    'Vector2D': [ '../API/Vector2DWrapper.cs' ],
    'Vector3D': [ '../API/Vector3DWrapper.cs' ],
    'Vertex': [ '../API/VertexWrapper.cs', '../API/MapElementWrapper.cs' ],
    'VisualCamera': [ '../API/VisualCameraWrapper.cs' ],
    'QueryOptions': [ '../QueryOptions.cs' ],
}

for topic in topics:
    texts = {
        'global': '',
        'properties': {},
        'constructors': {},
        'methods': {},
        'staticmethods': {}
    }    
    for filename in topics[topic]:
        topicname = filename.split('\\')[-1].replace('Wrapper.cs', '')

        with open(filename, 'r') as file:
            xmltext = ''
            parsingcomment = False
            incodeblock = False
            for line in file:
                line = line.strip()
                if line.startswith('///'):
                    parsingcomment = True
                    line = re.sub(r'^\t', r'', line.lstrip('/').lstrip(' '))
                    if line.startswith('```'):
                        if incodeblock:
                            xmltext += '```\n'
                            incodeblock = False
                        else:
                            xmltext += '\n```js\n'
                            incodeblock = True
                    else:
                        xmltext += line + '\n'
                elif parsingcomment is True:
                    commenttext = ''
                    d = xmltodict.parse('<d>' + xmltext + '</d>')['d']
                    summary = d['summary']
                    texttype, signature = determine_text_type(line)
                    if texttype == 'global':
                        texts['global'] = f'{summary}\n'
                    else:
                        commenttext += '\n---\n'
                        if 'version' in d:
                            commenttext += f'<span style="float:right;font-weight:normal;font-size:66%">Version: {d["version"]}</span>\n'
                        commenttext += f'### {signature}\n'

                        commenttext += f'{summary}\n'
                        if 'param' in d:
                            commenttext += '#### Parameters\n'
                            if isinstance(d['param'], list):
                                for p in d['param']:
                                    text = '*missing*'
                                    if '#text' in p:
                                        text = p['#text']
                                    commenttext += f'* {p["@name"]}: {text}\n'
                            else:
                                text ='*missing*'
                                if '#text' in d['param']:
                                    text = d['param']['#text'].replace('```', '\n```\n')
                                commenttext += f'* {d["param"]["@name"]}: {text}\n'
                        if 'returns' in d:
                            commenttext += '#### Return value\n'
                            text = '*missing*'
                            if d['returns'] is not None:
                                text = d['returns']
                            commenttext += f'{text}\n'

                        if signature not in texts[texttype]:
                            texts[texttype][signature] = ''
                        texts[texttype][signature] += commenttext
                    xmltext = ''
                    parsingcomment = False

    outfile = open(f'htmldoc/docs/{topic}.md', 'w')
    outfile.write(f'# {topic}\n\n')
    outfile.write(f'{texts["global"]}')
    if len(texts["constructors"]) > 0:
        outfile.write(f'## Constructors\n{get_sorted_comment_texts(texts["constructors"])}')
    if len(texts["staticmethods"]) > 0:
        outfile.write(f'## Static methods\n{get_sorted_comment_texts(texts["staticmethods"])}')
    if len(texts["properties"]) > 0:        
        outfile.write(f'## Properties\n{get_sorted_comment_texts(texts["properties"])}')
    if len(texts["methods"]) > 0:
        outfile.write(f'## Methods\n{get_sorted_comment_texts(texts["methods"])}')
    outfile.close()