import os
import requests

from dotenv import load_dotenv

load_dotenv()

headers = {
    "Accept": "application/vnd.github.v3+json",
    "Authorization": os.getenv("GITHUB_TOKEN"),
}

res = requests.get(f" https://api.github.com/repos/{os.getenv('GITHUB_REPOSITORY')}/actions/artifacts",
                   headers=headers).json()


def get_download_url(content):
    for artifact in content["artifacts"]:
        if artifact["name"] == "Build":
            run_id = artifact["workflow_run"]["id"]
            url = f"https://github.com/{os.getenv('GITHUB_REPOSITORY')}/actions/runs/{run_id}"
            return url
    return None


def message(url):
    content = {
        "username": "ビルドダウンロードページ",
        "content": "ビルドが終了しました。"
                   + f"\nダウンロードページ: {url}"
    }
    return content


requests.post(os.getenv("DISCORD_WEBHOOK_URL"), message(get_download_url(res)))