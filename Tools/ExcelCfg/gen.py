import exceltool

if '__main__' == __name__:
    exceltool.excel2xml(u"Excels/测试.xlsx", "./output/test.xml")
    exceltool.excel2xml(u"Excels/测试2.xlsx", "./output/test2.xml")
    exceltool.excel2xml(u"Excels/i18nAppStrings.xlsx", "./output/i18nAppStrings.xml")
    exceltool.excel2xml(u"Excels/UIPanelConfig.xlsx", "./output/UIPanelConfig.xml")

    exceltool.excel2lua(u"Excels/测试.xlsx", "./output/test.lua")
    exceltool.excel2lua(u"Excels/测试2.xlsx", "./output/test2.lua")
    exceltool.excel2lua(u"Excels/i18nAppStrings.xlsx", "./output/i18nAppStrings.lua")
    exceltool.excel2lua(u"Excels/UIPanelConfig.xlsx", "./output/UIPanelConfig.lua")

    exceltool.excel2json(u"Excels/测试.xlsx", "./output/test.json")
    exceltool.excel2json(u"Excels/测试2.xlsx", "./output/test2.json")
    exceltool.excel2json(u"Excels/i18nAppStrings.xlsx", "./output/i18nAppStrings.json")
    exceltool.excel2json(u"Excels/UIPanelConfig.xlsx", "./output/UIPanelConfig.json")


    print('done!')
    input('按任意键退出')