import zmq
import pandas as pd
import numpy as np

from xml.dom import minidom
import xml.etree.ElementTree as ET

def _execute_first():
    # サンプルデータを持つDataFrameを作成
    # data = {
    #     'Name': ['Alice', 'Bob', 'Charlie'],
    #     'Age': [25 + index, 30 + index, 35 + index],
    #     'City': ['New York', 'Los Angeles', 'Chicago']
    # }
    # df = pd.DataFrame(data)

    # 行数と列数の設定
    num_rows = 100
    num_cols = 5

    # ランダムなデータを生成
    data = np.random.rand(num_rows, num_cols)

    # 列名を設定
    columns = [f'col_{i}' for i in range(num_cols)]

    df = pd.DataFrame(data, columns=columns)
    return df

def _get_mode(root_node):
    return root_node.find('Mode').text

def _get_id(root_node):
    return root_node.find('Id').text

def _get_output_folder_path(root_node):
    return root_node.find('OutputFolderPath').text

def _get_formula_list(root_node):
    return [elem.text for elem in root_node.find('FormulaList')]

def _output_parquet(output_file_path, data_frame):
    data_frame.to_parquet(output_file_path, engine='pyarrow', index=False, use_dictionary=False)

def _response_success(output_file_path, index):
    response_root = ET.Element('root')
    status_node = ET.SubElement(response_root, 'Status')
    status_node.text = "Success"
    index_node = ET.SubElement(response_root, 'Index')
    index_node.text = str(index)
    output_file_path_node = ET.SubElement(response_root, 'OutputFilePath')
    output_file_path_node.text = output_file_path
    return minidom.parseString(ET.tostring(response_root, 'utf-8')).toprettyxml()

def _response_exception(exception):
    response_root = ET.Element('root')
    status_node = ET.SubElement(response_root, 'Status')
    status_node.text = "Exception"
    index_node = ET.SubElement(response_root, 'Index')
    index_node.text = str(index)
    exception_message = ET.SubElement(response_root, 'ExceptionMessage')
    exception_message.text = str(e)
    return minidom.parseString(ET.tostring(response_root, 'utf-8')).toprettyxml()

context = zmq.Context()

# REP ソケットを作成し、クライアントからの接続を待つ
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")

index = 1

while True:
    # クライアントからメッセージを受信
    message = socket.recv()
    print(f"Received request: {message.decode('utf-8')}")

    mode = ""
    
    try:
        # ET に変換
        root_node = ET.fromstring(message)

        # Mode
        mode = _get_mode(root_node)
        print(f"Mode: {mode}")

        # Id
        id = _get_id(root_node)
        print(f"Id: {id}")

        # OutputFolderPath
        output_folder_path = _get_output_folder_path(root_node)
        print(f"OutputFolderPath: {output_folder_path}")

        # FormulaList
        if (mode == "Calculate"):
            formula_list = _get_formula_list(root_node)
            for elem in formula_list:
                print(f"formula: {elem}")

    except Exception as e:
        socket.send(_response_exception(e))
        continue

    # dispatch
    df = pd.DataFrame()

    if mode.lower() == "CreateSampleData".lower():
        df = _execute_first()

    elif mode.lower() == "Calculate".lower():
        print("Calculate")

        # クライアントから受け取ったコードを実行
        df = _execute_first()

        try:
            for elem in formula_list:
                exec(elem)
        except Exception as e:
            socket.send(_response_exception(e).encode('utf-8'))
            continue

    print(df.head())

    # DataFrameをParquetファイルに出力
    output_file_path = output_folder_path + '/' + id + str(index) + '.parquet'
    _output_parquet(output_file_path, df)

    index = index + 1

    # クライアントに返信
    socket.send(_response_success(output_file_path, index).encode('utf-8'))
