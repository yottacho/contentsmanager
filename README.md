# Contents Manager

시리즈 컨텐츠 관리 프로그램

폴더에 아래 구조로 저장하는 컨텐츠를 관리한다.

```
* 컨텐츠명1
  * 001 컨텐츠명1-1
    * file1.abc
    * file2.abc
  * 002 컨텐츠명1-2
    * file1.abc
    * file2.abc
  * 003 컨텐츠명1-3
    * file1.abc
    * file2.abc
* 컨텐츠명2
  * 001 컨텐츠명2-1
    * file1.abc
    * file2.abc
  * 002 컨텐츠명2-2
    * file1.abc
    * file2.abc
```

귀찮으니 자세한 내용은 생략한다.

## 주요 기능

* 컨텐츠 목록에서 세부 컨텐츠가 몇 개인지, 언제 마지막으로 업데이트 했는 지 보여주고 정렬
* 세부 컨텐츠에 대해 폴더별로 번호매기기 및 번호 재정렬
* 다른 폴더에 있는 컨텐츠를 관리 폴더로 병합하는 기능

## To do

* 컨텐츠에 메모나 태그 달아서 관리
* 폴더 날짜 재정렬 (컨텐츠 파일의 가장 마지막 날짜로 컨텐츠 폴더 업데이트)
* 번호매기기 안 된 폴더 표시

## Build

* Microsoft(R) Visual Studio Community 2015
* .Net Framework 4.5.2
* C#

## Installation

Windows:

```
Download exe file and execute it
```

## Release History

* 1.1.1
    * 컨텐츠폴더 선택 히스토리 구현
    * moveform 더블클릭시 액션 추가
* 1.0.13
    * 이름 처리시 SQL Injection(') 관련 버그 수정
* 1.0.12
    * 이름 일부 일치할 경우 옵션을 구현해봤다
* 1.0.11
    * 삭제버튼 스크롤 옵션을 넣어봤다
* 1.0.10
    * 한동안 못잡고 있었던 스크롤 버그를 드디어 수정한 것 같은데 자신이 없다
* 1.0.9
    * UI 조금 개선
* 1.0.8
    * UI 마이너 개선(폴더 삭제시 스크롤 항목 유지)
    * [Bug] 이름 변경해서 이동 시 오류발생 및 매핑이 저장 안되는 이슈 해결
* 1.0.7
    * UI 마이너 개선(스크롤 처리방법 개선)
* 1.0.6
    * UI 마이너 개선(선택항목 유지처리, 스크롤 등)
* 1.0.5
    * 이동처리 후 선택항목 유지(마지막 선택한 항목에서 가장 가까운 것으로)
* 1.0.4
    * 관리자폼 업다운 버튼 기능 추가, 변경점 체크 시 커서를 wait 으로 변경
* 1.0.3
    * 전체 리프레시 작업을 백그라운드 처리로 변경
* 1.0.2
    * 디렉터리 찾지 못할 경우 발생하는 오류 수정
* 1.0.1
    * 쓰레드에서 UI에 억세스하는 버그 수정
    * 딕셔너리 개선
* 1.0.0
    * 첫 릴리즈

## Author

Yotta

Distributed under the GPLv3 license. See ``LICENSE`` for more information.

[https://github.com/yottacho/contentsmanager](https://github.com/yottacho/contentsmanager)

