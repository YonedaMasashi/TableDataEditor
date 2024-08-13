import zmq
import pandas as pd
import numpy as np

def execute_code(code):
    try:
        exec(code)
        return "Execution successful."
    except Exception as e:
        return str(e)


context = zmq.Context()

# REP ソケットを作成し、クライアントからの接続を待つ
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")

index = 1

while True:
    # クライアントからメッセージを受信
    message = socket.recv()
    print(f"Received request: {message.decode('utf-8')}")

    # サンプルデータを持つDataFrameを作成
    # data = {
    #     'Name': ['Alice', 'Bob', 'Charlie'],
    #     'Age': [25 + index, 30 + index, 35 + index],
    #     'City': ['New York', 'Los Angeles', 'Chicago']
    # }
    # df = pd.DataFrame(data)

    # 行数と列数の設定
    num_rows = 1000
    num_cols = 20

    # ランダムなデータを生成
    data = np.random.rand(num_rows, num_cols)

    # 列名を設定
    columns = [f'col_{i}' for i in range(num_cols)]

    df = pd.DataFrame(data, columns=columns)

    # クライアントから受け取ったコードを実行
    try:
        exec(message)
    except Exception as e:
        socket.send(str(e).encode('utf-8'))
        continue

    
    # DataFrameをParquetファイルに出力
    output_file_path = 'C:/Work/practice/Python/MQ/output_file_' + str(index) + '.parquet'
    df.to_parquet(output_file_path, engine='pyarrow', index=False, use_dictionary=False)

    index = index + 1

    # クライアントに返信
    socket.send(output_file_path.encode('utf-8'))
